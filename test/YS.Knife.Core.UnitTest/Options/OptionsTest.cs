﻿using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace YS.Knife.Options
{
    [TestClass]
    public class OptionsTest
    {
        [TestMethod]
        public void ShouldNotGetNullWhenNotDefineOptionsAttribute()
        {
            var provider = Utility.BuildProvider();
            var options = provider.GetService<IOptions<Custom0Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual(default, options.Value.Value);
        }

        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineOptionsAttributeWithEmptyConfigKey()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Custom1:Value"] = "some_value"
            });
            var options = provider.GetService<IOptions<Custom1Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("some_value", options.Value.Value);
        }

        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineOptionsAttributeConfigKey()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["C2:Value"] = "some_value"
            });
            var options = provider.GetService<IOptions<Custom2Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("some_value", options.Value.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(OptionsValidationException))]
        public void ShouldThrowExceptionWhenDefineDataAnnotationsAndConfigInvalidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Custom7:Value"] = "not a url value"
            });
            var options = provider.GetService<IOptions<Custom7Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
        }

        [TestMethod]
        public void ShouldGetExpectedValueWhenDefineDataAnnotationsAndConfigValidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Custom7:Value"] = "http://localhost"
            });
            var options = provider.GetService<IOptions<Custom7Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("http://localhost", options.Value.Value);
        }

        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineOptionsAttributeConfigKeyIsNested()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["C:B:D:Value"] = "some_value"
            });
            var options = provider.GetService<IOptions<Custom3Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("some_value", options.Value.Value);
        }
        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineOptionsAttributeConfigKeyIsNestedWithDot()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["C:B:D:Value"] = "some_value"
            });
            var options = provider.GetService<IOptions<Custom4Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("some_value", options.Value.Value);
        }
        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineOptionsAttributeConfigKeyIsNestedWithDoubleUnderScore()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["C:B:D:Value"] = "some_value"
            });
            var options = provider.GetService<IOptions<Custom5Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("some_value", options.Value.Value);
        }

        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineOptionsAttributeConfigKeyIsEmptyString()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Value"] = "some_value"
            });
            var options = provider.GetService<IOptions<Custom6Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("some_value", options.Value.Value);
        }

        [TestMethod]
        [ExpectedException(typeof(OptionsValidationException))]
        public void ShouldThrowExceptionWhenDefineCustomValidateAndConfigInvalidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Custom8:Number"] = "1"
            });
            var options = provider.GetService<IOptions<Custom8Options>>();
            Assert.IsNotNull(options);
            _ = options.Value;
        }
        [TestMethod]
        public void ShouldGetConfigedValueWhenDefineCustomValidateAndConfigValidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Custom8:Number"] = "2"
            });
            var options = provider.GetService<IOptions<Custom8Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
        }

        [TestMethod]
        public void ShouldGetPostedValueWhenDefineOptionsAttributeAndPostHandler()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["Custom9:Text"] = "some_text"
            });
            var options = provider.GetService<IOptions<Custom9Options>>();
            Assert.IsNotNull(options);
            Assert.IsNotNull(options.Value);
            Assert.AreEqual("__some_text", options.Value.Text);
        }

        [TestMethod]
        [ExpectedException(typeof(OptionsValidationException))]
        public void ShouldThrowExceptionWhenNestedObjectConfigInvalidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["DeepObject:Address:Province:Code"] = "invalidValue"
            });
            var options = provider.GetService<IOptions<DeepObjectOptions>>().Value;

        }

        [TestMethod]
        [ExpectedException(typeof(OptionsValidationException))]
        public void ShouldThrowExceptionWhenNestedListConfigInvalidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["DeepList:Addresses:0:Province:Code"] = "invalidValue"
            });
            var options = provider.GetService<IOptions<DeepListOptions>>().Value;

        }

        [TestMethod]
        [ExpectedException(typeof(OptionsValidationException))]
        public void ShouldThrowExceptionWhenNestedDicConfigInvalidValue()
        {
            var provider = Utility.BuildProvider(new Dictionary<string, string>
            {
                ["DeepDic:Addresses:name:Province:Code"] = "invalidValue"
            });
            var options = provider.GetService<IOptions<DeepDicOptions>>().Value;

        }
    }
}
