import type { LoaderFunction } from "@remix-run/node";
import { useLoaderData } from "@remix-run/react";

import {
  StarIcon,
  SearchIcon,
  ShoppingBagIcon,
} from "@heroicons/react/outline";
import SiteLayout from "~/components/SiteLayout";

import { getUserInfo, searchProduct, ProductSearchResult } from "~/lib/auth";

type LoaderData = { userInfo: any; productSearchResult: ProductSearchResult };

export const loader: LoaderFunction = async ({ request }) => {
  const userInfo = await getUserInfo(request);
  const productSearchResult = await searchProduct(request, "*", 10000, 1, 20);
  return { userInfo, productSearchResult };
};

export default function Index() {
  const data = useLoaderData<LoaderData>();
  return (
    <SiteLayout userInfo={data.userInfo}>
      <div className="container relative grid items-start gap-6 pt-4 pb-16 lg:grid-cols-4">
        {/* sidebar */}
        <div className="absolute left-4 top-16 z-10 col-span-1 w-72 overflow-hidden rounded bg-white px-4 pt-4 pb-6 shadow lg:static lg:block lg:w-full">
          <div className="relative space-y-5 divide-y divide-gray-200">
            {/* category filter */}
            <div className="relative">
              <h3 className="mb-3 text-xl font-medium uppercase text-gray-800">
                Categories
              </h3>
              <div className="space-y-2">
                {data.productSearchResult.categoryTags &&
                  data.productSearchResult.categoryTags.map((categoryTag) => (
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
                      <div className="ml-auto text-sm text-gray-600">
                        ({categoryTag.count})
                      </div>
                    </div>
                  ))}
              </div>
            </div>

            {/* inventory filter */}
            <div className="pt-4">
              <h3 className="mb-3 text-xl font-medium uppercase text-gray-800">
                Inventories
              </h3>
              <div className="space-y-2">
                {data.productSearchResult.inventoryTags &&
                  data.productSearchResult.inventoryTags.map((inventoryTag) => (
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
                      <div className="ml-auto text-sm text-gray-600">
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
              {data.productSearchResult.products &&
                data.productSearchResult.products.map((product) => (
                  <div
                    className="group overflow-hidden rounded bg-white shadow"
                    key={product.id}
                  >
                    {/* product image */}
                    <div className="relative">
                      <img
                        src={product.imageUrl}
                        className="w-full"
                        loading="lazy"
                      />
                      <div className="absolute inset-0 flex items-center justify-center gap-2 bg-black bg-opacity-40 opacity-0 transition group-hover:opacity-100">
                        <a
                          href={`/product/${product.id}`}
                          className="bg-primary flex h-9 w-9 items-center justify-center rounded-full text-lg text-white transition hover:bg-gray-800"
                        >
                          <SearchIcon className="h-5 w-5 text-red-500"></SearchIcon>
                        </a>
                        <a
                          href="#"
                          className="bg-primary flex h-9 w-9 items-center justify-center rounded-full text-lg text-white transition hover:bg-gray-800"
                        >
                          <ShoppingBagIcon className="h-5 w-5 text-red-500"></ShoppingBagIcon>
                        </a>
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
                      <div className="flex items-center">
                        <div className="flex gap-1 text-sm text-yellow-400">
                          <span>
                            <StarIcon className="h-5 w-5 text-yellow-500"></StarIcon>
                          </span>
                          <span>
                            <StarIcon className="h-5 w-5 text-yellow-500"></StarIcon>
                          </span>
                          <span>
                            <StarIcon className="h-5 w-5 text-yellow-500"></StarIcon>
                          </span>
                          <span>
                            <StarIcon className="h-5 w-5 text-yellow-500"></StarIcon>
                          </span>
                          <span>
                            <StarIcon className="h-5 w-5 text-yellow-500"></StarIcon>
                          </span>
                        </div>
                      </div>
                    </div>
                    {/* product content end */}
                    {/* product button */}
                    <a
                      href="#"
                      className="bg-primary border-primary hover:text-primary block w-full rounded-b border py-1 text-center text-white transition hover:bg-transparent"
                    >
                      Add to Cart
                    </a>
                    {/* product button end */}
                  </div>
                ))}
            </div>
          </div>
        </div>
      </div>
    </SiteLayout>
  );
}
