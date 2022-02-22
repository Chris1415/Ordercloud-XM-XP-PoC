using OrderCloud.SDK;
using System;
using System.Collections.Generic;

namespace BasicCompany.Foundation.Products.Ordercloud.Extensions
{
    public static class ApiRoleExtensions
    {
        public static ApiRole[] ToApiRoles(this string[] input)
        {
            var resultList = new List<ApiRole>();
            foreach (var inputElement in input)
            {
                if (Enum.TryParse(inputElement, out ApiRole mappedApiRole))
                {
                    resultList.Add(mappedApiRole);
                }
            }

            return resultList.ToArray();
        }
    }
}