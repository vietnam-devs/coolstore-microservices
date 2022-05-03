import type {
  LinksFunction,
  MetaFunction,
  LoaderFunction,
} from "@remix-run/node";
import {
  Links,
  LiveReload,
  Meta,
  Outlet,
  Scripts,
  ScrollRestoration,
  useCatch,
  useLoaderData,
} from "@remix-run/react";
import { getUserInfo, createUserSession } from "~/lib/auth";

import tailwind from "./tailwind.css";

export const links: LinksFunction = () => [
  { rel: "preconnect", href: "//fonts.gstatic.com", crossOrigin: "anonymous" },
  { rel: "stylesheet", href: tailwind },
];

export const meta: MetaFunction = () => ({
  charset: "utf-8",
  title: "CoolStore Web",
  viewport: "width=device-width,initial-scale=1",
});

export let loader: LoaderFunction = async ({ request }) => {
  const userInfo = await getUserInfo(request);
  if (userInfo != null) {
    const session = await createUserSession(userInfo.sub, "/");
    // console.info("root-cookie", session.headers.get("set-cookie"));
  }
  return null;
};

export default function App() {
  return (
    <Document>
      <Layout>
        <Outlet />
      </Layout>
    </Document>
  );
}

function Document({
  children,
  title,
}: {
  children: React.ReactNode;
  title?: string;
}) {
  let data = useLoaderData();

  return (
    <html lang="en">
      <head>
        {title ? <title>{title}</title> : null}
        <Meta />
        <Links />
      </head>
      <body>
        {children}
        <ScrollRestoration />
        <Scripts />
        <LiveReload />
      </body>
    </html>
  );
}

function Layout({ children }: React.PropsWithChildren<{}>) {
  return (
    <div className="remix-root remix-app">
      {children}
    </div>
  );
}

export function CatchBoundary() {
  let caught = useCatch();

  let message;
  switch (caught.status) {
    case 401:
      message = (
        <p>
          Oops! Looks like you tried to visit a page that you do not have access
          to.
        </p>
      );
      break;
    case 404:
      message = (
        <p>Oops! Looks like you tried to visit a page that does not exist.</p>
      );
      break;
    case 500:
      message = (
        <p>Oops! Something serious happened.</p>
      );

    default:
      throw new Error(caught.data || caught.statusText);
  }

  return (
    <Document title={`${caught.status} ${caught.statusText}`}>
      <Layout>
        <h1>
          {caught.status}: {caught.statusText}
        </h1>
        {message}
      </Layout>
    </Document>
  );
}

export function ErrorBoundary({ error }: { error: Error }) {
  return (
    <Document title="Error!">
      <Layout>
        <div>
          <h1>There was an error</h1>
          <p>{error.message}</p>
          <hr />
          <p>
            Hey, developer, you should replace this with what you want your
            users to see. <a href="/login" className="text-red-500">Log In</a> to the system please!
          </p>
        </div>
      </Layout>
    </Document>
  );
}
