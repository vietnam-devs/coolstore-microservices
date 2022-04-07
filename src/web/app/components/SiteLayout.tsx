import { PropsWithChildren, ReactElement } from "react";

import AppHeader from "./AppHeader";
import AppFooter from "./AppFooter";

type SiteLayoutProps = {
  userInfo: any;
};

function SiteLayout({
  children,
  userInfo,
}: PropsWithChildren<SiteLayoutProps>): ReactElement {
  return (
    <div className="remix-app">
      <AppHeader userInfo={userInfo} />
      <div className="remix-app__main">
        <div className="remix-app__main-content container mx-auto">
          {children}
        </div>
      </div>
      <AppFooter></AppFooter>
    </div>
  );
}

export default SiteLayout;
