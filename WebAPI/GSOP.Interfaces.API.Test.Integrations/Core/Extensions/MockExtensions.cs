namespace GSOP.Interfaces.API.Test.Integrations.Core.Extensions
{
    internal static class MockExtensions
    {
        /// <summary>
        /// Verifies that all veriiable methods were called and no others
        /// </summary>
        /// <typeparam name="T">Mocked service type</typeparam>
        public static void VerifyStrongly<T>(this Mock<T> mock) where T : class
        {
            mock.Verify();
            mock.VerifyNoOtherCalls();
        }
    }
}
