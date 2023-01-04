# Corpay One Task Id Analyzer

This Roslyn Analyzer has a single purpose, to detect usages of the `System.Threading.Tasks.Task.Id` property and flag it as an error.

If you're doing advanced `Task` manipulation logic then this task isn't for you. For everyone else this package will save you from incorrectly referencing the id of the `Task` type rather than what is generally meant, the id of the entity returned by `Task`.