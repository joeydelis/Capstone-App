using MauiApp1.Classes;

namespace UnitTestProject
{
    public class UnitTest1
    {

        private readonly Firebase firebase;
        private readonly MockSecureStorageService secureStorageService;
        private readonly string testEmail = "test123@gmail.com";
        private readonly string testPassword = "test123";
        public UnitTest1()
        {
            secureStorageService = new MockSecureStorageService();
            firebase = new Firebase(secureStorageService);
        }

        [Fact]
        public async Task SignInAsync_Test_True()
        {
            var result = await firebase.SignInAsync(testEmail, testPassword);

            Assert.True(result);
        }

        [Fact]
        public async Task SignInAsync_Test_False()
        {
            var result = await firebase.SignInAsync(testEmail, "theWrongPassword");

            Assert.False(result);
        }

        [Fact]
        public async Task SignUpAsync_Test_False()
        {
            var result = await firebase.SignUpAsync(testEmail, testPassword);

            Assert.False(result);
        }

        [Fact]
        public async Task GetUserPresets_Test_True()
        {
            await firebase.SignInAsync(testEmail, testPassword);

            var presets = await firebase.GetUserPresetsAsync();

            Assert.NotNull(presets);
            Assert.NotEmpty(presets);
        }

        [Fact]
        public async Task IsIdTokenExpired_Test_True()
        {

            var result = await firebase.IsIdTokenExpired("eyJhbGciOiJSUzI1NiIsImtpZCI6ImEwODA2N2Q4M2YwY2Y5YzcxNjQyNjUwYzUyMWQ0ZWZhNWI2YTNlMDkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vY2Fwc3RvbmUtZmI2YjAiLCJhdWQiOiJjYXBzdG9uZS1mYjZiMCIsImF1dGhfdGltZSI6MTc0MjAxMzQyMiwidXNlcl9pZCI6IjFqTDF3SmV0WExac1g5WW5PbFh2MDB3TUhSaDEiLCJzdWIiOiIxakwxd0pldFhMWnNYOVluT2xYdjAwd01IUmgxIiwiaWF0IjoxNzQyMDEzNDIyLCJleHAiOjE3NDIwMTcwMjIsImVtYWlsIjoidGVzdDEyM0BnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsidGVzdDEyM0BnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.LV_ETcgxJm5Z41ilGR7PHTzzvHMGauoOUsI2aPVvXcRWmwJszKkiPU8w94XI-0_egmFEdDYLQGJNndCSR3giEninIYg-bZ-nK6WNaDd2e0NhA-vBPL7ziN5jlT4R1b7HJzTD-HahVx4MKou7FpCmhFAUURwxEA6cEsGUBxgMDgsVZYX9XtxR0jfFyQbn4aq-DRRE35-ub7uT2Zmr__OM7cXXm5LG-w52ycOF9sbk9hNVJBycK9uwdCJUwYDLS8XF53_UXMR7LddkWtNuFGW6gb6y_o033NO7K86O7odR_h3xBf4x0GXDrAXdqTmIKwtPO0ea6MUiuBdj4aNhVMW0VA");
        
            Assert.True(result);
        }

        [Fact]
        public async Task IsIdTokenExpired_Test_False()
        {
            await firebase.SignInAsync(testEmail, testPassword);

            var token = await secureStorageService.GetAsync("firebase_token");

            var result = await firebase.IsIdTokenExpired(token);
            
            Assert.False(result);
        }

        [Fact]
        public async Task RefreshIdToken_Test_True()
        {
            await firebase.SignInAsync(testEmail, testPassword);

            await secureStorageService.SetAsync("firebase_token", "eyJhbGciOiJSUzI1NiIsImtpZCI6ImEwODA2N2Q4M2YwY2Y5YzcxNjQyNjUwYzUyMWQ0ZWZhNWI2YTNlMDkiLCJ0eXAiOiJKV1QifQ.eyJpc3MiOiJodHRwczovL3NlY3VyZXRva2VuLmdvb2dsZS5jb20vY2Fwc3RvbmUtZmI2YjAiLCJhdWQiOiJjYXBzdG9uZS1mYjZiMCIsImF1dGhfdGltZSI6MTc0MjAxMzQyMiwidXNlcl9pZCI6IjFqTDF3SmV0WExac1g5WW5PbFh2MDB3TUhSaDEiLCJzdWIiOiIxakwxd0pldFhMWnNYOVluT2xYdjAwd01IUmgxIiwiaWF0IjoxNzQyMDEzNDIyLCJleHAiOjE3NDIwMTcwMjIsImVtYWlsIjoidGVzdDEyM0BnbWFpbC5jb20iLCJlbWFpbF92ZXJpZmllZCI6ZmFsc2UsImZpcmViYXNlIjp7ImlkZW50aXRpZXMiOnsiZW1haWwiOlsidGVzdDEyM0BnbWFpbC5jb20iXX0sInNpZ25faW5fcHJvdmlkZXIiOiJwYXNzd29yZCJ9fQ.LV_ETcgxJm5Z41ilGR7PHTzzvHMGauoOUsI2aPVvXcRWmwJszKkiPU8w94XI-0_egmFEdDYLQGJNndCSR3giEninIYg-bZ-nK6WNaDd2e0NhA-vBPL7ziN5jlT4R1b7HJzTD-HahVx4MKou7FpCmhFAUURwxEA6cEsGUBxgMDgsVZYX9XtxR0jfFyQbn4aq-DRRE35-ub7uT2Zmr__OM7cXXm5LG-w52ycOF9sbk9hNVJBycK9uwdCJUwYDLS8XF53_UXMR7LddkWtNuFGW6gb6y_o033NO7K86O7odR_h3xBf4x0GXDrAXdqTmIKwtPO0ea6MUiuBdj4aNhVMW0VA");

            await firebase.RefreshIdTokenAsync();

            var token = await secureStorageService.GetAsync("firebase_token");

            var result = await firebase.IsIdTokenExpired(token);

            Assert.False(result);
        }


        [Fact]
        public async Task AddDeleteUserPresets_Test_True()
        {
            await firebase.SignInAsync(testEmail, testPassword);

            var addResult = await firebase.AddUserSetting("testCase");

            var presets = await firebase.GetUserPresetsAsync();
            string presetId = null;
            foreach(var preset in presets)
            {
                if(preset.Name == "testCase")
                {
                    presetId = preset.Id; 
                    break;
                }
            }

            var deleteResult = await firebase.DeleteUserPresetAsync(presetId);

            
            Assert.True(deleteResult);
            Assert.NotNull(presetId);
            Assert.True(addResult);

        }

        [Fact]
        public async Task IsUserSignedIn_Test_True()
        {
            await firebase.SignInAsync(testEmail, testPassword);

            var result = await firebase.IsUserSignedInAsync();

            Assert.True(result);
        }

        [Fact]
        public async Task IsUserSignedIn_Test_False()
        {

            var result = await firebase.IsUserSignedInAsync();

            Assert.False(result);
        }

        [Fact]
        public async Task Logout_Test()
        {
            await firebase.SignInAsync(testEmail, testPassword);

            firebase.Logout();

            var result = await secureStorageService.GetAsync("firebase_uuid");

            Assert.Null(result);
        }

    }
}