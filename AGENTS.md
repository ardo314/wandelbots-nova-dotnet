# Repository workflow instructions

These instructions apply to every contributor and automated coding agent working in this repository.

## Branch and pull request policy

- Never commit changes directly to `main`.
- Create a separate branch for every issue or logically independent change.
- Keep each branch focused on one issue or change.
- Use a descriptive branch name, such as `feat/nats-client`, `fix/streaming-auth`, or `docs/release-process`.
- Push the branch and create a pull request targeting `main`.
- Include the related issue in the pull request description when one exists.
- Ensure generation, build, and tests pass before requesting review.
- Do not merge the pull request until the repository owner, `ardo314`, has explicitly approved the merge.
- After opening or updating a pull request, stop and wait for that approval. Silence, passing CI, or an approval from automation is not permission to merge.

## Generated code

- Update the source specification or generation tooling rather than manually editing generated files when practical.
- Run `./scripts/Generate-Client.ps1`, `dotnet build`, and `dotnet test` before requesting review.
- Explain any Kiota warnings or nondeterministic generated changes in the pull request.
