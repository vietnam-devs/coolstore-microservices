import { ActionFunction, LoaderFunction, redirect } from "@remix-run/node";
import { Form, useLoaderData } from "@remix-run/react";
import Image from "remix-image";
import SiteLayout from "~/components/SiteLayout";
import {
  getUserInfo,
  getCartForCurrentUser,
  checkout,
  CartModel,
} from "~/lib/auth";

type LoaderData = { userInfo: any; cart: CartModel };

export const loader: LoaderFunction = async ({ request }) => {
  const userInfo = await getUserInfo(request);
  const { cartData } = await getCartForCurrentUser(request);
  return { userInfo, cart: cartData };
};

export const action: ActionFunction = async ({ request }) => {
  const _ = await checkout(request);
  return redirect("/");
};

export default function Index() {
  const { userInfo, cart } = useLoaderData<LoaderData>();
  return (
    <SiteLayout userInfo={userInfo} cartItemCount={cart.items.length}>
      <Form method="post">
        <div className="container grid-cols-12 items-start gap-6 pb-16 pt-4 lg:grid">
          <div className="lg:col-span-8 xl:col-span-9">
            <div className="mb-4 hidden bg-gray-200 py-2 pl-12 pr-20 md:flex xl:pr-28">
              <p className="text-center text-gray-600">Product</p>
              <p className="ml-auto mr-16 text-center text-gray-600 xl:mr-24">
                Quantity
              </p>
              <p className="text-center text-gray-600">Total</p>
            </div>

            <div className="space-y-4">
              {cart.items.map((item) => (
                <div className="flex flex-wrap items-center gap-4 rounded border border-gray-200 p-4 md:flex-nowrap md:justify-between md:gap-6">
                  <div className="w-32 flex-shrink-0">
                    <Image
                      src={item.productImagePath}
                      className="w-full"
                    ></Image>
                  </div>
                  <div className="w-full md:w-1/3">
                    <h2 className="textl-lg mb-3 font-medium uppercase text-gray-800 xl:text-xl">
                      {item.productDescription}
                    </h2>
                    <p className="text-primary font-semibold">
                      ${item.productPrice}
                    </p>
                  </div>
                  <div className="flex divide-x divide-gray-300 border border-gray-300 text-gray-600">
                    <div className="flex h-8 w-8 cursor-pointer select-none items-center justify-center text-xl">
                      -
                    </div>
                    <div className="flex h-8 w-10 items-center justify-center">
                      {item.quantity}
                    </div>
                    <div className="flex h-8 w-8 cursor-pointer select-none items-center justify-center text-xl">
                      +
                    </div>
                  </div>
                  <div className="ml-auto md:ml-0">
                    <p className="text-primary text-lg font-semibold">
                      ${item.productPrice * item.quantity}
                    </p>
                  </div>
                  <div className="hover:text-primary cursor-pointer text-gray-600">
                    <i className="fas fa-trash"></i>
                  </div>
                </div>
              ))}
            </div>
          </div>
          <div className="mt-6 rounded border border-gray-200 px-4 py-4 lg:col-span-4 lg:mt-0 xl:col-span-3">
            <h4 className="mb-4 text-lg font-medium uppercase text-gray-800">
              ORDER SUMMARY
            </h4>
            <div className="space-y-1 border-b border-gray-200 pb-3 text-gray-600">
              <div className="flex justify-between font-medium">
                <p>Subtotal</p>
                <p>${cart.cartItemTotal}</p>
              </div>
              <div className="flex justify-between">
                <p>Delivery</p>
                <p>Free</p>
              </div>
              <div className="flex justify-between">
                <p>Tax</p>
                <p>Free</p>
              </div>
            </div>
            <div className="my-3 flex justify-between font-semibold uppercase text-gray-800">
              <h4>Total</h4>
              <h4>${cart.cartTotal}</h4>
            </div>

            <button className="bg-primary border-primary block w-full rounded-b border bg-red-500 py-1 text-center font-bold text-white transition hover:bg-red-800">
              Process to checkout
            </button>
          </div>
        </div>
      </Form>
    </SiteLayout>
  );
}
