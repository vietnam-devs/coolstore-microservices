import { PropsWithChildren, ReactElement } from "react";

import AppHeader from "./AppHeader";
import AppFooter from "./AppFooter";

type SiteLayoutProps = {
  userInfo: any;
  cartItemCount: number;
};

function SiteLayout({
  children,
  userInfo,
  cartItemCount,
}: PropsWithChildren<SiteLayoutProps>): ReactElement {
  return (
    <div className="remix-app">
      <AppHeader userInfo={userInfo} cartItemCount={cartItemCount} />
      <div className="remix-app__main">
        <div className="remix-app__main-content container mx-auto">
          <div className="relative bg-cover bg-center bg-no-repeat py-10">
            <div className="container">
              <h1 className="mb-4 text-4xl font-medium text-gray-800 md:text-5xl xl:text-6xl">
                CoolStore buits with modern technologies
              </h1>
              <p className="text-base leading-6 text-gray-600">
                Lorem ipsum dolor sit amet, consectetur adipisicing elit. Culpa
                assumenda aliquid inventore nihil laboriosam odio
              </p>
            </div>
          </div>
          {children}
        </div>
      </div>
      <AppFooter></AppFooter>
    </div>
  );
}

export default SiteLayout;
