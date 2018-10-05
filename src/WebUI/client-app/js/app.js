require("./log.js");
require("./../styles/app.css");

const config = require("./../../config/config.json");

///////////////////////////////
// config
///////////////////////////////
Oidc.Log.logger = console;
Oidc.Log.level = Oidc.Log.DEBUG;

const settings = {
  authority: config.authorityServer,
  client_id: "implicit",
  redirect_uri: `${config.webServer}/callback`,
  post_logout_redirect_uri: `${config.webServer}`,
  response_type: "id_token token",
  scope: "openid profile email api",

  popup_redirect_uri: `${config.webServer}/callback`,
  popup_post_logout_redirect_uri: `${config.webServer}`,

  silent_redirect_uri: `${config.webServer}/silent`,
  automaticSilentRenew: true,
  //silentRequestTimeout:10000,

  filterProtocolClaims: true,
  loadUserInfo: true
};

const mgr = new Oidc.UserManager(settings);

///////////////////////////////
// behaviors
///////////////////////////////
logToConsole = (message) => {
  console.log(message);
  return true;
};

getUser = async () => {
  var user = await mgr.getUser();
  console.info(user);
  if (user == null) return {};
  return {
    accessToken: user.access_token,
    tokenType: user.token_type,
    Scope: user.scope,
    Profile: {
      UserId: user.profile.sub,
      Name: user.profile.name,
      Email: user.profile.email,
      Website: user.profile.website
    }
  };
};

startSignin = async () => {
  await mgr.signinRedirect();
};

startSignOut = async (idToken) => {
  return await mgr.signoutRedirect({ id_token_hint: idToken });
};

callback = async () => {
  return await mgr.signinRedirectCallback();
};

silent = async () => {
  return await mgr.signinSilentCallback();
};

refresh = (href) => {
  location.reload(href);
  return true;
};

///////////////////////////////
// events
///////////////////////////////
mgr.events.addAccessTokenExpiring(() => {
  console.log("token expiring");
  log("token expiring");
});

mgr.events.addAccessTokenExpired(() => {
  console.log("token expired");
  log("token expired");
});

mgr.events.addSilentRenewError((e) => {
  console.log("silent renew error", e.message);
  log("silent renew error", e.message);
});

mgr.events.addUserLoaded((user) => {
  console.log("user loaded", user);
  mgr.getUser().then(() => {
    console.log("getUser loaded user after userLoaded event fired");
  });
});

mgr.events.addUserUnloaded((e) => {
  console.log("user unloaded");
});
