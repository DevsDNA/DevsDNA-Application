namespace DevsDNA.Application.Tests.Extensions
{
    using DevsDNA.Application.Extensions;
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    [TestClass]
    public class ExtensionMethodsString
    {
        [TestMethod]
        public void GetImagePathWhenHtmlContainsOneImage()
        {
            string path = "This html contains <img src=\"/image.png\"/> a single image".GetPathFromFirstImage();

            Assert.AreEqual("/image.png", path);
        }

        [TestMethod]
        public void GetFirstImagePathWhenHtmlContainsTwoImages()
        {
            string path = "This html contains <img src=\"/image.png\"/> two <img src=\"/other_image.png\"/> images".GetPathFromFirstImage();

            Assert.AreEqual("/image.png", path);
        }

        [TestMethod]
        public void ReturnsNullIfHtmlNotContainsImages()
        {
            string path = "This html does not contains images".GetPathFromFirstImage();

            Assert.IsNull(path);
        }

        [TestMethod]
        public void ReturnsNullIfHtmlContainsOneImageWithoutSrc()
        {
            string path = "This html contains <img class=\"image-class\"/> an image but no src".GetPathFromFirstImage();

            Assert.IsNull(path);
        }

        [TestMethod]
        public void GetSecondImagePathWhenFirstImageNotContainsSrc()
        {
            string path = "this html contains <img class=\"image-class\"/> two <img src=\"/image.png\"/> images but only the second has src".GetPathFromFirstImage();

            Assert.AreEqual("/image.png", path);
        }
    }
}
