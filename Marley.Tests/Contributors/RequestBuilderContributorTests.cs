using System;
using System.Collections.Generic;
using System.Net;
using Marley.Contributors;
using Marley.Pipeline;
using Marley.Pipeline.Context;
using NUnit.Framework;

namespace Marley.Tests.Contributors
{
    [TestFixture]
    public class RequestBuilderContributorTests
    {
        private IApiContext _context;

        [Test]
        public void given_a_valid_context_the_context_request_is_set_to_a_WebRequest()
        {
            const string method = "GET", uri = "http://foo.com";

            given_the_context( new ApiRequest { HttpMethod = method, Uri = uri, Timeout = 10 } );

            the_context_should_be(
                c => c is WebRequest, 
                c => c.Method == method, 
                c => c.Timeout == 10, 
                c => c.RequestUri.OriginalString == uri,
                c => c.Proxy == null
                );
        }

        [Test]
        public void if_headers_were_set_in_the_context_then_they_should_be_added_to_the_WebRequest()
        {
            given_the_context(new ApiRequest {HttpMethod = "GET", Uri = "http://foo.com", Headers = new Dictionary<string, string> {{"Foo", "Bar"}}});

            the_context_should_be(
                c => c.Headers["Foo"] == "Bar"
                );
        }

        [Test]
        public void if_UseFiddler_is_true_then_proxy_is_set()
        {
            const string method = "GET", uri = "http://foo.com";

            given_the_context(new ApiContext { UseFiddler = true, Request = new ApiRequest { HttpMethod = method, Uri = uri, Timeout = 10 } });

            the_context_should_be(
                c => c.Proxy != null
                );
        }

        [Test]
        public void if_the_uri_is_not_set_throw_a_InvalidContextException()
        {
            given_the_context(new ApiRequest { HttpMethod = "GET" });
            it_should_throw<InvalidContextException>("uri");
        }

        [Test]
        public void if_the_http_method_is_not_set_throw_a_InvalidContextException()
        {
            given_the_context(new ApiRequest { Uri = "http://foo.com" });
            it_should_throw<InvalidContextException>("http method");
        }

        [Test]
        public void if_invalid_uri_throw_a_InvalidContextException()
        {
            given_the_context(new ApiRequest { Uri = "fdsfdkl", HttpMethod = "GET" });
            it_should_throw<InvalidContextException>("invalid uri");
        }

        private void it_should_throw<T>(string substringInMessage)
            where T : Exception
        {
            try
            {
                new RequestBuilderContributor().Execute(_context);

                Assert.Fail("A {0} should have been thrown", typeof(T).Name);
            }
            catch (Exception ex)
            {
                Assert.IsInstanceOf(typeof(T), ex);
                StringAssert.Contains(substringInMessage, ex.Message.ToLower());
            }
        }

        private void the_context_should_be(params Predicate<WebRequest>[] validateRequest)
        {
            Assert.AreNotEqual(PipelineContinuation.Abort, new RequestBuilderContributor().Execute(_context));

            foreach (var request in validateRequest)
            {
                Assert.IsTrue(request(_context.Request.Request));
            }
        }

        private void given_the_context(ApiRequest context)
        {
            given_the_context(new ApiContext {Request = context});
        }

        private void given_the_context(IApiContext context)
        {
            _context = context;
        }
    }
}