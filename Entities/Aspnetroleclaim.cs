using System;
using System.Collections.Generic;

namespace sampleApp.Entities;

public partial class Aspnetroleclaim
{
    public int Id { get; set; }

    public int RoleId { get; set; }

    public string? ClaimType { get; set; }

    public string? ClaimValue { get; set; }

    public virtual Aspnetrole Role { get; set; } = null!;
}
