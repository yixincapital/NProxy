﻿//
// NProxy is a library for the .NET framework to create lightweight dynamic proxies.
// Copyright © Martin Tamme
//
// This program is free software: you can redistribute it and/or modify
// it under the terms of the GNU Lesser General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
//
// This program is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
// GNU Lesser General Public License for more details.
//
// You should have received a copy of the GNU Lesser General Public License
// along with this program. If not, see <http://www.gnu.org/licenses/>.
//

using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace NProxy.Core.Internal.Reflection
{
    /// <summary>
    /// Provides <see cref="EventInfo"/> extension methods.
    /// </summary>
    internal static class EventInfoExtensions
    {
        /// <summary>
        /// Returns a value indicating whether the specified event is overrideable.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <returns>A value indicating whether the specified event is overrideable.</returns>
        public static bool CanOverride(this EventInfo eventInfo)
        {
            if (eventInfo == null)
                throw new ArgumentNullException("eventInfo");

            var methodInfos = eventInfo.GetAllAccessors();

            return methodInfos.All(m => m.CanOverride());
        }

        /// <summary>
        /// Returns all accessors for the specified event.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <returns>All accessors for the specified event.</returns>
        public static IEnumerable<MethodInfo> GetAllAccessors(this EventInfo eventInfo)
        {
            if (eventInfo == null)
                throw new ArgumentNullException("eventInfo");

            var methodInfos = new List<MethodInfo>
            {
                eventInfo.GetAddMethod(true),
                eventInfo.GetRemoveMethod(true)
            };

            var raiseMethodInfo = eventInfo.GetRaiseMethod(true);

            if (raiseMethodInfo != null)
                methodInfos.Add(raiseMethodInfo);

            var otherMethodInfo = eventInfo.GetOtherMethods(true);

            methodInfos.AddRange(otherMethodInfo);

            return methodInfos;
        }

        /// <summary>
        /// Returns the full name of the specified event.
        /// </summary>
        /// <param name="eventInfo">The event information.</param>
        /// <returns>The full name.</returns>
        public static string GetFullName(this EventInfo eventInfo)
        {
            if (eventInfo == null)
                throw new ArgumentNullException("eventInfo");

            var fullName = new StringBuilder();

            fullName.Append(eventInfo.DeclaringType);
            fullName.Append(Type.Delimiter);
            fullName.Append(eventInfo.Name);

            return fullName.ToString();
        }
    }
}