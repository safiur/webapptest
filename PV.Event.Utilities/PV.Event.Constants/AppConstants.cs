namespace PV.Event.Constants
{
    /// <summary>
    /// Proview Constants
    /// </summary>
    public static class AppConstants
    {
        //public const string AttestationServiceBusConnectionString = "AzureSettings:ServiceBusConnectionString";
        //public const string AttestationServiceBusTopic = "AzureSettings:ServiceBusTopic";
        //public const string AttestationSubscriptionName = "AzureSettings:SubscriptionName";
        //public const string DefaultConnectionString = "ConnectionStrings:DefaultConnectionString";
        //public const string DefaultConnectionString1 = "ConnectionStrings:DefaultConnectionString1";
        //public const string DefaultConnectionString2 = "ConnectionStrings:DefaultConnectionString2";
        //public const string DefaultConnectionString3 = "ConnectionStrings:DefaultConnectionString3";
        //public const string DefaultConnectionString4 = "ConnectionStrings:DefaultConnectionString4";
        //public const string DmsConnectionString = "ConnectionStrings:DMSConnection";
        //public const string TinHashContextConnectionString = "ConnectionStrings:TinHashContext";
        
        
        //public static string SaltValue = "SaltValue";
        //public static string JsonFileContainer = "AzureBlobSettings:JsonFileContainer";
        //public static string BlobStoragePath = "AzureBlobSettings:BlobStoragePath";
        //public static string DAConnectionString= "ConnectionStrings:DAConnection";
        //public static string ProviewMaskingFields = "ProviewMaskingFields";

        /////////////////////////////////////////////////////////////
        
        public static string Subscriptions = "Subscriptions";
        public static string EmailTemplatePath = "EmailTemplate/EmailTemplateView";
        public static string InValidFirstName = "First name is not valid";
        public static string InValidEmailAddress = "Email address is not valid";
        public static string LoggerTracePath = "Logs/Trace-{Date}.txt";
        public static string LoggerErrorPath = "Logs/Trace-{Date}.txt";
        public static string ProviewMaskingFields = "ProviewMaskingFields";
        public static string RegexMaskingKeysforScanJSON = "RegexMaskingKeysforScanJSON";
        public static string MaskDataFlag = "MaskData";
        public static string BlobUploadFlag = "BlobUpload";

        ///////////////////////////////////////////////////////////////////////////////////////////
        public static string AllowedHosts = "AllowedHosts";
        public static string ConnectionStringsDefaultConnectionString =
            "ConnectionStrings:DefaultConnectionString";
        public static string ConnectionStringsDAConnection =
            "ConnectionStrings:DAConnection";
        public static string ConnectionStringsDMSConnection =
            "ConnectionStrings:DMSConnection";
        public static string AzureSettingsServiceBusConnectionString = "AzureSettings:ServiceBusConnectionString";
        public static string AzureSettingsServiceBusTopic = "AzureSettings:ServiceBusTopic";
        public static string ApiKeys = "ApiKeys";
        public static string MaskData = "MaskData";
        public static string SaltValue = "SaltValue";
        public static string JSONFileStore = "JSONFileStore";
        public static string BlobUpload = "BlobUpload";
        public const string AzureSettingsMaxPublisherRetryCount = "AzureSettings:MaxPublisherRetryCount";
        public static string AzureBlobSettingsJsonFileContainer = "AzureBlobSettings:JsonFileContainer";


        //////////////////////////PV.Event.Attestation.DLQMessageHandler///////////////////////////////////

        public static string AzureSettingsEmailScheduleTriggerTime = "AzureSettings:EmailScheduleTriggerTime";
        public static string AzureSettingsDLQSubscriptionName = "AzureSettings:DLQSubscriptionName";
        public static string MailSettingsProvider = "MailSettings:Provider";
        public static string MailSettingsFromName = "MailSettings:FromName";
        public static string MailSettingsFromEmail = "MailSettings:FromEmail";
        public static string MailSettingsRecipients = "MailSettings:Recipients";
        public static string MailSettingsServerAddress = "MailSettings:ServerAddress";
        public static string MailSettingsServerPort = "MailSettings:ServerPort";
        public static string MailSettingsenableSsl = "MailSettings:enableSsl";
        public static string MailSettingsSubject = "Dead Letter Queue Messages Notification";
        public static string MailSettingsPassword = "MailSettings:Password";
        public static string MailSettingsUserName = "MailSettings:UserName";


        //////////////////////////ProView.Attestation.Publisher///////////////////////////////////
        
        public static string ConnectionStringsPrPubDefaultConnectionString =
            "ConnectionStrings:PrPubDefaultConnectionString";

        //////////////////////////DA.DataExtract.Subscriber///////////////////////////////////

        public static string AzureSettingsDASubSubscriptionName = "AzureSettings:DASubSubscriptionName";
        public static string AzureBlobSettingsDASubBlobStoragePath = "AzureBlobSettings:DASubBlobStoragePath";
        public static string AzureBlobSettingsDASubJsonFileContainer = "AzureBlobSettings:DASubJsonFileContainer";

        //////////////////////////DA.Extended.DataExtract.Subscriber///////////////////////////////////
       
        public static string AzureSettingsDAExtSubSubscriptionName = "AzureSettings:DAExtSubSubscriptionName";
        public static string AzureBlobSettingsDAExtSubBlobStoragePath = "AzureBlobSettings:DAExtSubBlobStoragePath";
        public static string AzureBlobSettingsDAExtSubJsonFileContainer = "AzureBlobSettings:DAExtSubJsonFileContainer";
        

        //////////////////////////ProView.DataExtract.Subscriber///////////////////////////////////

        
        public static string AzureSettingsPrDESSubscriptionName = "AzureSettings:PrDESSubscriptionName";
        public static string AzureBlobSettingsPrDESBlobStoragePath = "AzureBlobSettings:PrDESBlobStoragePath";
        public static string AzureBlobSettingsPrDESJsonFileContainer = "AzureBlobSettings:PrDESJsonFileContainer";


        //////////////////////////ProView.Extended.DataExtract.Subscriber///////////////////////////////////
        
        
        public static string AzureSettingsPrExtSubSubscriptionName = "AzureSettings:PrExtSubSubscriptionName";
        public static string AzureBlobSettingsPrExtSubBlobStoragePath = "AzureBlobSettings:PrExtSubBlobStoragePath";
        public static string AzureBlobSettingsPrExtSubJsonFileContainer = "AzureBlobSettings:PrExtSubJsonFileContainer";

        //////////////////////////Attestation.Event.Subscriber///////////////////////////////////
       
        public static string AzureSettingsAtEvtSubSubscriptionName = "AzureSettings:AtEvtSubSubscriptionName";
    }

    /// <summary>
    /// API Status Constants
    /// </summary>
    public static class StatusConstants
    {
        public const string SuccessText = "SUCCESS";
        public const string BadRequestText = "BAD REQUEST";
		public const string PublisherSuccessMessage = "Message has been published successfully";
    }

    /// <summary>
    /// Error Messages Constants
    /// </summary>
    public static class ErrorMessages
    {
        public const string NoAttestationJsonId = "'AttestationJsonId' cannot be empty.";
        public const string NoAttestationId = "'AttestationId' cannot be empty.";
		public const string NoCAQHId = "'CAQHId' cannot be empty.";
		public const string NoAttestationDateTimeStamp = "'AttestationDateTimeStamp' cannot be empty.";
	}
}
