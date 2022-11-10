﻿using Financity.Domain.Common;
using Microsoft.AspNetCore.Identity;

namespace Financity.Domain.Entities;

public sealed class User : IdentityUser<Guid>
{
}