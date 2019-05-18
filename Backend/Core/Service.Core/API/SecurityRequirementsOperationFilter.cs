namespace Service.Core
{
    #region Using Statements

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Microsoft.AspNetCore.Authorization;
    using Service.Core.API;
    using Swashbuckle.AspNetCore.Swagger;
    using Swashbuckle.AspNetCore.SwaggerGen;

    #endregion

    /// <summary>
    ///     https://github.com/mattfrear/Swashbuckle.AspNetCore.Filters/tree/master/src/Swashbuckle.AspNetCore.Filters
    /// </summary>
    public class SecurityRequirementsOperationFilter : IOperationFilter
    {
        private readonly SecurityRequirementsOperationFilter<AuthorizeAttribute> _filter;

        /// <summary>
        ///     Constructor for SecurityRequirementsOperationFilter
        /// </summary>
        /// <param name="includeUnauthorizedAndForbiddenResponses">
        ///     If true (default), then 401 and 403 responses will be added to
        ///     every operation
        /// </param>
        public SecurityRequirementsOperationFilter(bool includeUnauthorizedAndForbiddenResponses = true)
        {
            Func<IEnumerable<AuthorizeAttribute>, IEnumerable<string>> policySelector = authAttributes =>
                authAttributes
                    .Where(a => !string.IsNullOrEmpty(a.Policy))
                    .Select(a => a.Policy);

            _filter = new SecurityRequirementsOperationFilter<AuthorizeAttribute>(policySelector, includeUnauthorizedAndForbiddenResponses);
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            _filter.Apply(operation, context);
        }
    }

    public class SecurityRequirementsOperationFilter<T> : IOperationFilter where T : Attribute
    {
        // inspired by https://github.com/domaindrivendev/Swashbuckle.AspNetCore/blob/master/test/WebSites/OAuth2Integration/ResourceServer/Swagger/SecurityRequirementsOperationFilter.cs

        private readonly bool _includeUnauthorizedAndForbiddenResponses;
        private readonly Func<IEnumerable<T>, IEnumerable<string>> _policySelector;

        /// <summary>
        ///     Constructor for SecurityRequirementsOperationFilter
        /// </summary>
        /// <param name="policySelector">Used to select the authorization policy from the attribute e.g. (a => a.Policy)</param>
        /// <param name="includeUnauthorizedAndForbiddenResponses">
        ///     If true (default), then 401 and 403 responses will be added to
        ///     every operation
        /// </param>
        public SecurityRequirementsOperationFilter(Func<IEnumerable<T>, IEnumerable<string>> policySelector, bool includeUnauthorizedAndForbiddenResponses = true)
        {
            _policySelector = policySelector;
            _includeUnauthorizedAndForbiddenResponses = includeUnauthorizedAndForbiddenResponses;
        }

        public void Apply(Operation operation, OperationFilterContext context)
        {
            if (context.GetControllerAndActionAttributes<AllowAnonymousAttribute>().Any())
            {
                return;
            }

            var actionAttributes = context.GetControllerAndActionAttributes<T>();

            if (!actionAttributes.Any())
            {
                return;
            }

            if (_includeUnauthorizedAndForbiddenResponses)
            {
                operation.Responses.Add("401", new Response { Description = "Unauthorized" });
                operation.Responses.Add("403", new Response { Description = "Forbidden" });
            }

            var policies = _policySelector(actionAttributes) ?? Enumerable.Empty<string>();

            operation.Security = new List<IDictionary<string, IEnumerable<string>>>
            {
                new Dictionary<string, IEnumerable<string>>
                {
                    {"oauth2", policies}
                }
            };
        }
    }
}