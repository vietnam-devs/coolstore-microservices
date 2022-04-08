import Axios from 'axios'
import https from "https";
import { createCookieSessionStorage, redirect } from "@remix-run/node";

export type ProductModel = {
  id: string
  name: string
  price: number
  imageUrl: string
  description: string
}

export type ProductDetailModel = {
  id: string
  name: string
  price: number
  imageUrl: string
  description: string
  categoryId: string
  categoryName: string
  inventoryId: string
  inventoryLocation: string
}

export type TagModel = {
  key: string
  text: string
  count: number
}

export type ProductSearchResult = {
  products: ProductModel[]
  categoryTags: TagModel[]
  inventoryTags: TagModel[]
  totalItem: number
  page: number
}

const API_URL = "https://localhost:5000";
const PRODUCT_URL = `${API_URL}/api-gw/product-catalog/api/products`;
const PRODUCT_SEARCH_URL = `${API_URL}/api-gw/product-catalog/api/products/search`;

const axios = Axios.create({
  httpsAgent: new https.Agent({
    rejectUnauthorized: false,
  }),
});

const storage = createCookieSessionStorage({
  cookie: {
    name: "coolstore_session",
    // secure doesn't work on localhost for Safari
    // https://web.dev/when-to-use-local-https/
    secure: process.env.NODE_ENV === "production",
    secrets: ['secret'],
    sameSite: "lax",
    path: "/",
    maxAge: 60 * 60 * 24 * 30,
    httpOnly: true,
  },
});

export const getUserInfo = async (request: Request) => {
  const { data } = await axios.get<any>(
    `${API_URL}/userinfo`
    , {
      headers: {
        cookie: request.headers.get("Cookie")?.toString()
      } as any
    });
  if (Object.keys(data).length === 0) {
    return null;
  }
  return data;
}

export async function searchProduct(request: Request, query: string, price: number, page: number, pageSize: number = 10) {
  const { data } = await axios.get<any>(
    `${PRODUCT_SEARCH_URL}/${price}/${page}/${pageSize}`
    , {
      headers: {
        cookie: request.headers.get("Cookie")?.toString()
      } as any
    });

  const result = {
    products: data.results,
    categoryTags: data.categoryTags,
    inventoryTags: data.inventoryTags,
    page: data.page,
    totalItem: data.total
  } as ProductSearchResult;

  //console.log(result)
  return result;
}

export async function getProductById(request: Request, id: string) {
  console.log("product_id", id);
  const { data } = await axios.get<any>(
    `${PRODUCT_URL}/${id}`
    , {
      headers: {
        cookie: request.headers.get("Cookie")?.toString()
      } as any
    });

  // console.log(data as ProductDetailModel);
  return data as ProductDetailModel;
}

export async function createUserSession(userId: string, redirectTo: string) {
  const session = await storage.getSession();
  session.set("userId", userId);
  return redirect(redirectTo, {
    headers: {
      "Set-Cookie": await storage.commitSession(session),
    },
  });
}
