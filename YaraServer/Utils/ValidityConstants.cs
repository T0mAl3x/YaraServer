using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace YaraServer.Utils
{
    public enum ValidityConstants
    {
        OK,
        NOT_SIGNED_CA,
        NOT_VALID_YET_OR_EXPIRED,
        IS_REVOKED,
        NOT_THUMBPRINT_CA
    }
}
