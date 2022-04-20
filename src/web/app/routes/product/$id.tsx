import type { LoaderFunction } from "@remix-run/node";
import { useLoaderData } from "@remix-run/react";
import { ShoppingBagIcon, HeartIcon } from "@heroicons/react/outline";
import Image from "remix-image";

import SiteLayout from "~/components/SiteLayout";

import {
  getUserInfo,
  getProductById,
  ProductDetailModel,
  CartModel,
  getCartForCurrentUser,
} from "~/lib/auth";

type LoaderData = {
  userInfo: any;
  product: ProductDetailModel;
  cart: CartModel;
};

export const loader: LoaderFunction = async ({ request, params }) => {
  const userInfo = await getUserInfo(request);
  const product = await getProductById(request, params.id as string);
  const { cartData } = await getCartForCurrentUser(request);
  return { userInfo, product, cart: cartData };
};

export default function ProductDetail() {
  const { userInfo, product, cart } = useLoaderData<LoaderData>();

  return (
    <SiteLayout userInfo={userInfo} cartItemCount={cart.items.length}>
      <div className="container grid gap-6 pt-4 pb-6 lg:grid-cols-2">
        <div>
          <div>
            <Image id="main-img" src={product.imageUrl} className="w-full" />
          </div>
          <div className="mt-4 grid grid-cols-5 gap-4"></div>
        </div>
        <div>
          <h2 className="mb-2 text-2xl font-medium uppercase md:text-3xl">
            {product.name}
          </h2>
          <div className="mb-4 flex items-center">
            <div className="flex gap-1 text-sm text-yellow-400">
              <span>
                <i className="fas fa-star"></i>
              </span>
              <span>
                <i className="fas fa-star"></i>
              </span>
              <span>
                <i className="fas fa-star"></i>
              </span>
              <span>
                <i className="fas fa-star"></i>
              </span>
              <span>
                <i className="fas fa-star"></i>
              </span>
            </div>
          </div>

          <div className="space-y-2">
            <p className="space-x-2">
              <span className="font-semibold text-gray-800">Category: </span>
              <span className="text-gray-600">{product.categoryName}</span>
            </p>
            <p className="space-x-2">
              <span className="font-semibold text-gray-800">Inventory: </span>
              <span className="text-gray-600">{product.inventoryLocation}</span>
            </p>
          </div>

          <div className="mt-4 flex items-baseline gap-3">
            <span className="text-primary text-xl font-semibold">
              ${product.price}
            </span>
          </div>

          <p className="mt-4 text-gray-600">{product.description}</p>

          <div className="mt-6 flex gap-3 border-b border-gray-200 pb-5">
            <a
              href="#"
              className="bg-primary border-primary hover:text-primary flex items-center rounded border border-gray-300 px-8 py-2
                    text-sm font-medium uppercase transition hover:bg-transparent"
            >
              <span className="mr-2">
                <ShoppingBagIcon className="h-5 w-5 text-red-500"></ShoppingBagIcon>
              </span>{" "}
              Add to cart
            </a>
            <a
              href="#"
              className="bg-primary border-primary hover:text-primary flex items-center rounded border border-gray-300 px-8 py-2
              text-sm font-medium uppercase transition hover:bg-transparent"
            >
              <span className="mr-2">
                <HeartIcon className="h-5 w-5 text-pink-500"></HeartIcon>
              </span>{" "}
              Wishlist
            </a>
          </div>
        </div>
      </div>
    </SiteLayout>
  );
}
