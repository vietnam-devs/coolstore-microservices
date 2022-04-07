import type {
  LoaderFunction,
} from "@remix-run/node";
import { useLoaderData } from "@remix-run/react";

import SiteLayout from "~/components/SiteLayout";

import { getUserInfo, getProductById, ProductModel } from "~/lib/auth";

type LoaderData = { userInfo: any; product: ProductModel };

export const loader: LoaderFunction = async ({ request, params }) => {
  const userInfo = await getUserInfo(request);
  const product = await getProductById(request, params.productId as string);
  return { userInfo, product };
}

export default function ProductDetail() {
  const data = useLoaderData<LoaderData>();

  return (
    <SiteLayout userInfo={data.userInfo}>
      <h1>detail</h1>
    </SiteLayout>
  );
}
