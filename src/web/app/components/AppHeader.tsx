import { PropsWithChildren, ReactElement } from "react";

type LoaderData = { userInfo: any };

function AppHeader({ userInfo }: PropsWithChildren<LoaderData>): ReactElement {
  return (
    <div className="remix-app__header">
      <div className="mx-auto container">
        <div className="flex items-center justify-between border-b-2 border-gray-100 py-6 md:justify-start md:space-x-10">
          <div className="flex justify-start lg:w-0 lg:flex-1">
            <a href="/">
              <span className="sr-only">CoolStore</span>
              <img
                className="h-8 w-auto sm:h-10"
                src="https://tailwindui.com/img/logos/workflow-mark-indigo-600.svg"
                alt=""
              />
            </a>
          </div>

          <div className="hidden items-center justify-end md:flex md:flex-1 lg:w-0">
            <span>{userInfo?.name}</span>
            {userInfo == null && (
              <a
                href="https://localhost:5000/login"
                className="ml-8 inline-flex items-center justify-center whitespace-nowrap rounded-md border border-transparent bg-indigo-600 px-4 py-2 text-base font-medium text-white shadow-sm hover:bg-indigo-700"
              >
                Sign in
              </a>
            )}
            {userInfo != null && (
              <a
                href="https://localhost:5000/logout"
                className="ml-8 inline-flex items-center justify-center whitespace-nowrap rounded-md border border-transparent bg-red-600 px-4 py-2 text-base font-medium text-white shadow-sm hover:bg-red-700"
              >
                Sign Out
              </a>
            )}
          </div>
        </div>
      </div>
    </div>
  );
}

export default AppHeader;
