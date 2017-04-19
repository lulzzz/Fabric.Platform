﻿namespace Fabric.Platform.Shared
{
    public static class Constants
    {
        public static class FabricHeaders
        {
            public const string SubjectNameHeader = "fabric-end-user-subject-id";
            public const string IdTokenHeader = "fabric-end-user";
            public const string CorrelationTokenHeaderName = "correlation-token";
            public const string AuthenticationHeaderPrefix = "Bearer";
        }

        public static class FabricLogContextProperties
        {
            public const string CorrelationTokenContextName = "CorrelationToken";
            public const string SubjectName = "FabricEndUserSubjectId";
        }
    }
}
