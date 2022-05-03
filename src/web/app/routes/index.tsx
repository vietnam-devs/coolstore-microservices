import {
  ActionFunction,
  json,
  LoaderFunction,
  redirect,
} from "@remix-run/node";
import { Form, Link, useLoaderData } from "@remix-run/react";
import { SearchIcon } from "@heroicons/react/outline";
import Image from "remix-image";

import SiteLayout from "~/components/SiteLayout";
import {
  getUserInfo,
  updateCartForCurrentUser,
  searchProduct,
  ProductSearchResult,
  CartModel,
  getCartForCurrentUser,
} from "~/lib/auth";

type LoaderData = {
  userInfo: any;
  productSearchResult: ProductSearchResult;
  cart: CartModel;
};

export const loader: LoaderFunction = async ({ request }) => {
  const userInfo = await getUserInfo(request);
  const { productSearchResult } = await searchProduct(
    request,
    "*",
    10000,
    1,
    20
  );
  const { cartData } = await getCartForCurrentUser(request);
  return json({ userInfo, productSearchResult, cart: cartData });
};

export const action: ActionFunction = async ({ request }) => {
  const formData = await request.formData();
  const _ = await updateCartForCurrentUser(
    request,
    formData.get("productId")?.toString()!
  );
  return redirect("/");
};

export default function Index() {
  const { userInfo, productSearchResult, cart } = useLoaderData<LoaderData>();

  return (
    <SiteLayout userInfo={userInfo} cartItemCount={cart.items.length}>
      <div className="container relative grid items-start gap-6 pt-4 pb-16 lg:grid-cols-4">
        {/* sidebar */}
        <div className="absolute left-4 top-16 z-10 col-span-1 w-72 overflow-hidden rounded bg-white px-4 pt-4 pb-6 shadow lg:static lg:block lg:w-full">
          <div className="relative space-y-5 divide-y divide-gray-200">
            {/* category filter */}
            <div className="relative">
              <h3 className="mb-3 text-xl font-medium uppercase text-red-800">
                Category
              </h3>
              <div className="space-y-2">
                {productSearchResult.categoryTags &&
                  productSearchResult.categoryTags.map((categoryTag) => (
                    <div className="flex items-center" key={categoryTag.key}>
                      <input
                        type="checkbox"
                        id={categoryTag.key}
                        className="text-primary cursor-pointer rounded-sm focus:ring-0"
                      />
                      <label
                        htmlFor={categoryTag.key}
                        className="ml-3 cursor-pointer text-gray-600"
                      >
                        {categoryTag.text}
                      </label>
                      <div className="ml-auto text-sm text-rose-900">
                        ({categoryTag.count})
                      </div>
                    </div>
                  ))}
              </div>
            </div>

            {/* inventory filter */}
            <div className="pt-4">
              <h3 className="mb-3 text-xl font-medium uppercase text-red-800">
                Inventory
              </h3>
              <div className="space-y-2">
                {productSearchResult.inventoryTags &&
                  productSearchResult.inventoryTags.map((inventoryTag) => (
                    <div className="flex items-center" key={inventoryTag.key}>
                      <input
                        type="checkbox"
                        id={inventoryTag.key}
                        className="text-primary cursor-pointer rounded-sm focus:ring-0"
                      />
                      <label
                        htmlFor={inventoryTag.key}
                        className="ml-3 cursor-pointer text-gray-600"
                      >
                        {inventoryTag.text}
                      </label>
                      <div className="ml-auto text-sm text-rose-900">
                        ({inventoryTag.count})
                      </div>
                    </div>
                  ))}
              </div>
            </div>
          </div>
        </div>

        {/* products */}
        <div className="col-span-3">
          <div className="grid gap-6">
            {/* product wrapper */}
            <div className="grid gap-6 sm:grid-cols-2 lg:grid-cols-2 xl:grid-cols-3">
              {/* single product */}
              {productSearchResult.products &&
                productSearchResult.products.map((product) => (
                  <Form method="post">
                    <input type="hidden" name="productId" value={product.id} />
                    <div
                      className="group overflow-hidden rounded bg-white shadow"
                      key={product.id}
                    >
                      {/* product image */}
                      <div className="relative">
                        <Image
                          src={product.imageUrl}
                          className="w-full"
                        ></Image>
                        <div className="absolute inset-0 flex items-center justify-center gap-2 bg-black bg-opacity-40 opacity-0 transition group-hover:opacity-100">
                          <Link
                            prefetch="intent"
                            to={`/product/${product.id}`}
                            className="bg-primary flex h-9 w-9 items-center justify-center rounded-full text-lg text-white transition hover:bg-gray-800"
                          >
                            <SearchIcon className="h-5 w-5 text-red-500"></SearchIcon>
                          </Link>
                        </div>
                      </div>
                      {/* product image end */}
                      {/* product content */}
                      <div className="px-4 pt-4 pb-3">
                        <a href={`/product/${product.id}`}>
                          <h4 className="hover:text-primary mb-2 text-xl font-medium uppercase text-gray-800 transition">
                            {product.name}
                          </h4>
                        </a>
                        <div className="mb-1 flex items-baseline space-x-2">
                          <p className="text-primary font-roboto text-xl font-semibold">
                            ${product.price}
                          </p>
                        </div>
                      </div>
                      {/* product content end */}
                      {/* product button */}
                      <button className="bg-primary border-primary block w-full rounded-b border bg-red-500 py-1 text-center font-bold text-white transition hover:bg-red-800">
                        Add to Cart
                      </button>
                      {/* product button end */}
                    </div>
                  </Form>
                ))}
            </div>
          </div>
        </div>
      </div>
    </SiteLayout>
  );
}
