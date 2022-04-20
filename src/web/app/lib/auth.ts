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

export type CartModel = {
  id: string
  userId: string
  cartItemTotal: number
  cartItemPromoSavings: number
  shippingTotal: number
  shippingPromoSavings: number
  cartTotal: number
  isCheckOut: boolean
  items: CartItem[]
}

export type CartItem = {
  quantity: number
  price: number
  productId: string
  productName: string
  productPrice: number
  productDescription: string
  productImagePath: string
  inventoryId: string
  inventoryLocation: string
  inventoryDescription: string
  inventoryWebsite: string
}

const API_URL = process.env.API_URL || "https://localhost:5000";
const PRODUCT_URL = `${API_URL}/api-gw/product-catalog/api/products`;
const PRODUCT_SEARCH_URL = `${API_URL}/api-gw/product-catalog/api/products/search`;
const CART_URL = `${API_URL}/api-gw/shopping-cart/api/carts`;

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
      headers: getHeaders(request)
    });
  if (Object.keys(data).length === 0) {
    return null;
  }
  return data;
}

export async function searchProduct(request: Request, query: string, price: number, page: number, pageSize: number = 10) {
  const response = await axios.get<any>(
    `${PRODUCT_SEARCH_URL}/${price}/${page}/${pageSize}`
    , {
      headers: getHeaders(request)
    });

  const { data } = response;

  const result = {
    products: data.results,
    categoryTags: data.categoryTags,
    inventoryTags: data.inventoryTags,
    page: data.page,
    totalItem: data.total
  } as ProductSearchResult;

  return { productSearchResult: result };
}

export async function getProductById(request: Request, id: string) {
  const { data } = await axios.get<any>(
    `${PRODUCT_URL}/${id}`
    , {
      headers: getHeaders(request)
    });
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

export async function getCartForCurrentUser(request: Request) {
  const response = await axios.get<any>(
    CART_URL
    , {
      headers: getHeaders(request)
    });

  const { data } = response;

  // console.log(data as CartModel);
  return { cartData: data as CartModel };
}

export async function updateCartForCurrentUser(request: Request, productId: string) {
  const userData = await getUserInfo(request);
  const { cartData } = await getCartForCurrentUser(request);

  if (cartData.id === null) {
    // create new cart
    const { data } = await axios.post<any>(
      CART_URL,
      {
        productId: productId,
        userId: userData.userId,
        quantity: 1,
      },
      {
        headers: getHeaders(request, true)
      });
    return data as CartModel;
  } else {
    // update cart
    const { data } = await axios.put<any>(
      CART_URL,
      {
        productId: productId,
        quantity: 1,
      },
      {
        headers: getHeaders(request, true)
      });
    return data as CartModel;
  }
}

export async function checkout(request: Request) {
  const response = await axios.put<any>(
    `${CART_URL}/checkout`
    , {}, {
    headers: getHeaders(request, true)
  });

  const { data } = response;

  return { cartData: data as CartModel };
}

function getHeaders(request: Request, isCsrf = false) {
  const cookie = request.headers.get("Cookie")?.toString()!;
  if (isCsrf) {
    const csrf = convertCookie(cookie)['XSRF-TOKEN'];
    return {
      cookie: cookie,
      "X-XSRF-TOKEN": csrf,
    } as any;
  } else {
    return {
      cookie: cookie
    } as any;
  }
}

function convertCookie(cookie: string) {
  const str: string[] | any = cookie?.toString().split('; ');
  const result: any = {};
  for (let i in str) {
    const cur = str[i].split('=');
    result[cur[0]] = cur[1];
  }
  return result;
}
