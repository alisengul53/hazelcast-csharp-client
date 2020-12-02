﻿// Copyright (c) 2008-2020, Hazelcast, Inc. All Rights Reserved.
//
// Licensed under the Apache License, Version 2.0 (the "License");
// you may not use this file except in compliance with the License.
// You may obtain a copy of the License at
//
// http://www.apache.org/licenses/LICENSE-2.0
//
// Unless required by applicable law or agreed to in writing, software
// distributed under the License is distributed on an "AS IS" BASIS,
// WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
// See the License for the specific language governing permissions and
// limitations under the License.

using System.Collections.Generic;
using System.Collections.Immutable;
using System.Composition;
using System.Linq;
using Microsoft.DocAsCode.Build.Common;
using Microsoft.DocAsCode.Build.ManagedReference;
using Microsoft.DocAsCode.Common;
using Microsoft.DocAsCode.Plugins;

namespace Hazelcast.Net.DocAsCode
{
    [Export(nameof(ManagedReferenceDocumentProcessor), typeof(IDocumentBuildStep))]
    // ReSharper disable once UnusedMember.Global -- injected
    public class GatherHazelcastOptions : BaseDocumentBuildStep
    {
        public override string Name => nameof(GatherHazelcastOptions);
        public override int BuildOrder => State.GatherBuilderOrder;

        [Import]
        public HazelcastOptionsState State { get; set; }

        public override IEnumerable<FileModel> Prebuild(ImmutableList<FileModel> models, IHostService host)
        {
            static bool IsOptionsModel(string modelKey)
                => modelKey.StartsWith("~/obj/dev/api/") &&
                   modelKey.EndsWith("Options.yml");

            Logger.LogInfo("Gathering options.");
            State.OptionFiles.AddRange(models.Where(x => IsOptionsModel(x.Key)));
            Logger.LogInfo($"Gathered {State.OptionFiles.Count} options.");

            foreach (var optionFile in State.OptionFiles)
                Logger.LogInfo($"Options: {optionFile.Key}");

            State.Gathered.Set();

            return models;
        }
    }
}
