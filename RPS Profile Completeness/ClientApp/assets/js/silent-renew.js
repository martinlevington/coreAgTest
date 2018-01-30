var silentRenew = (function(){

  var init =  function () {
    console.log('silentRenew.init()');
    return new Oidc.UserManager({}).signinSilentCallback();
  };

  return {
    init: init,
  }

}());

silentRenew.init();

