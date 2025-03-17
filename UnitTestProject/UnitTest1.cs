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
    }
}