﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using static Arch.EntityLayer.Entities.Auth.Authorization;

namespace Arch.EntityLayer.Entities
{
    public class ProjectFilePath
    {
        public int Id { get; set; }

        public int CompetitionId { get; set; }
        public Competition Competition { get; set; }

        public string? DesignerId { get; set; }
        [JsonIgnore]
        public AppUser Designer { get; set; }

        public string Address { get; set; }

        public int? Type { get; set; }
    }
}
