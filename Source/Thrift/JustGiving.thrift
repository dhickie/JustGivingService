namespace csharp JustGivingThrift
namespace java justGivingThrift

struct ServiceConfig
{
	1: required string pageId,
	2: required i32 pollingPeriod,
	3: required string rainmeterExe
}

service ConfigService
{
	/**
	 * Sets the current service configuration
	 */
	void SetConfiguration(1: ServiceConfig newConfig),
	
	/**
	 * Gets the current service configuration
	 */
	ServiceConfig GetConfiguration()
}