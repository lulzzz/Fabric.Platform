﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using LibOwin;
using Serilog.Core;
using Serilog.Events;
using AppFunc = System.Func<System.Collections.Generic.IDictionary<string, object>, System.Threading.Tasks.Task>;

namespace Fabric.Platform.Logging
{
    public class DiagnosticsMiddleware
    {
        private readonly LoggingLevelSwitch _levelSwitch;
        private readonly AppFunc _next;

        private static readonly PathString DiagnosticsPath = new PathString("/_diagnostics");
        private const string LogLevelParameter = "LogLevel";

        public DiagnosticsMiddleware(AppFunc next, LoggingLevelSwitch levelSwitch)
        {
            _levelSwitch = levelSwitch ?? throw new ArgumentNullException(nameof(levelSwitch));
            _next = next ?? throw new ArgumentNullException(nameof(next));
        }

        public Task Inject(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            if (context.Request.Path.StartsWithSegments(DiagnosticsPath))
            {
                return HandleDiagnosticsEndpoint(context);
            }
            return _next(env);
        }

        private Task HandleDiagnosticsEndpoint(OwinContext context)
        {
            var logLevelFromQuery = context.Request.Query[LogLevelParameter];

            if (Enum.TryParse(logLevelFromQuery, true, out LogEventLevel logLevel))
            {
                _levelSwitch.MinimumLevel = logLevel;
                context.Response.StatusCode = 204;
                return Task.FromResult(0);
            }
            context.Response.StatusCode = 400;
            return Task.FromResult(0);
        }
    }
}
