# Dependencies

<!-- Maintained by /emmz:architecture; verified by `make deps`. -->

| Tool | Why | Version | Check | Auth check | Install / auth |
|---|---|---|---|---|---|
| dotnet | build, test, benchmarks (targets net10.0 and net11.0) | >=11.0.100-rc.1 (pinned in `global.json`; re-pin to 11.0.100 at GA) | `dotnet --version` | — | macOS: `brew install --cask dotnet-sdk@preview` or the installer from https://dotnet.microsoft.com/download/dotnet/11.0 |
| .NET 10 runtime | run net10.0 tests and benchmarks | >=10.0 | `dotnet --list-runtimes \| grep -q 'Microsoft.NETCore.App 10\.'` | — | Installed with the .NET 10 SDK: `brew install --cask dotnet-sdk` |
| Rosetta 2 | Grpc.Tools ships only `macosx_x64` protoc/plugin binaries; needed on Apple Silicon | any | `arch -x86_64 /usr/bin/true` | — | `softwareupdate --install-rosetta --agree-to-license` |
| openspec | change tracking | any | `openspec --version` | — | `npm i -g @fission-ai/openspec` |
| git | version control | any | `git --version` | — | `brew install git` |
| gh | public GitHub repo, PRs | any | `gh --version` | `gh auth status` | `brew install gh`, then `gh auth login` |
