import { LoaderFunction } from "@remix-run/node";
import { useLoaderData } from "@remix-run/react";
import SiteLayout from "~/components/SiteLayout";
import { getUserInfo, getCartForCurrentUser } from "~/lib/auth";

type LoaderData = { userInfo: any };

export const loader: LoaderFunction = async ({ request }) => {
  const userInfo = await getUserInfo(request);
  const data = await getCartForCurrentUser(request);
  return { userInfo };
};

export default function Index() {
  const data = useLoaderData<LoaderData>();
  return (
    <SiteLayout userInfo={data.userInfo}>
      <h1>Cart</h1>
    </SiteLayout>
  );
}
