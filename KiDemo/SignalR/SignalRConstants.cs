namespace KiDemo.SignalR;

public static class SignalRConstants
{
    public const string UrlChatHub = "/ChatHub";

	public const string OnRootResults = "OnRootResults";
	public const string OnServiceState = "OnServiceState";
	public const string OnQuery = "OnQuery";
	public const string OnQueryProcessed = "OnQueryProcessed";
	public const string OnStatisticValues = "OnStatisticValues";
	public const string OnRestError = "OnRestError";

	public const string CommandStart = "CommandStart";
	public const string CommandStop = "CommandStop";
	public const string CommandClientMessage = "CommandClientMessage";

	public const string CommandRelease = "CommandRelease";
	public const string CommandSetAiModel = "CommandSetAiModel";
	public const string CommandWorkerCount = "CommandWorkerCount";
	public const string CommandSkipNextQuery = "CommandSkipNextQuery";
	public const string CommandClearQueryQueue = "CommandClearQueryQueue";
	public const string CommandBreakDuration = "CommandBreakDuration";

	public const string LoadClientStructure = "LoadClientStructure";

	public const string GetNextQuery = "GetNextQuery";
	public const string ExecuteDirectRequest = "ExecuteDirectRequest";

	public const string LoadClient = "LoadClient";
	public const string LoadQueryMessageText = "LoadQueryMessageText";

	public const string UpdateClient = "UpdateClient";
	public const string CreateClient = "CreateClient";
	public const string DeleteClient = "DeleteClient";
	public const string SaveMemo = "SaveMemo";
	public const string DeleteMemo = "DeleteMemo";

	public const string LoadFunctions = "LoadFunctions";
	public const string LoadUsings = "LoadUsings";
	public const string SaveFunction = "SaveFunction";
	public const string DeleteFunction = "DeleteFunction";
	public const string ExecuteFunction = "ExecuteFunction";
	public const string SaveUsing = "SaveUsing";
	public const string DeleteUsing = "DeleteUsing";

	public const string LoadWorkflows = "LoadWorkflows";
	public const string SaveWorkflow = "SaveWorkflow";
	public const string DeleteWorkflow = "DeleteWorkflow";
	public const string RenderWorkflow = "RenderWorkflow";

	public const string GetWorkflowConstants = "GetWorkflowConstants";
	public const string SaveWorkflowConstants = "SaveWorkflowConstants";

	public const string OnUserMessage = "OnUserMessage";
	public const string OnErrorMessage = "OnErrorMessage";
}