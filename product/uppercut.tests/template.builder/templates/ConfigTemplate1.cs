namespace uppercut.tests.template.builder.templates
{
    public class ConfigTemplate1
    {
        public static string Contents =
@"<?xml version=""1.0"" encoding=""utf-8"" ?>
<configuration>
  <configSections>
    <section name=""castle"" type=""Castle.Windsor.Configuration.AppDomain.CastleSectionHandler, Castle.Windsor""/>
    <section name=""log4net"" type=""log4net.Config.Log4NetConfigurationSectionHandler, log4net""/>
  </configSections>

  <appSettings>
    <add key=""AssemblyDirectory"" value=""${dirs.app.toplevel}\BIntegration\"" />
    <add key=""StubFileDirectory"" value=""CSTUB\""/>
    <add key=""BondStubFileInput"" value=""STUB_BondTrades.csv""/>
    <add key=""SwapStubFileInput"" value=""STUB_Swaps.csv""/>
    <add key=""UseSwapStubFile"" value=""True""/>
    <add key=""OptionStubFileInput"" value=""STUB_Options.csv""/>
    <add key=""UseOptionStubFile"" value=""True""/>
    <add key=""ScheduleStubFileInput"" value=""STUB_Schedules.csv""/>
    <add key=""UseScheduleStubFile"" value=""true""/>
    <add key=""DiscountNoteStubFileInput"" value=""STUB_DiscountNotes.csv""/>
    <add key=""UseDiscountNoteStubFile"" value=""True""/>
    <add key=""ArchivePath"" value=""${b.folders.archive.path}""/>
  </appSettings>

  <connectionStrings>
    <add name=""APlace"" connectionString=""Server=${server.database};initial catalog=APlace;Integrated Security=SSPI"" providerName=""System.Data.SqlClient""/>
    <add name=""Yep"" connectionString=""Server=${server.database};initial catalog=Yep;Integrated Security=SSPI"" providerName=""System.Data.SqlClient""/>
  </connectionStrings>

  <castle>

    <facilities>
      <!--<facility id=""masstransit"" type=""MassTransit.WindsorIntegration.MassTransitFacility, MassTransit.WindsorIntegration"">
        <bus id=""local"" endpoint=""msmq://${server.queue}/${b.queue.name}"">
          <subscriptionCache mode=""local"" />
          <subscriptionService endpoint=""msmq://${server.mtpubsub}/${mtpubsub.queue.name}"" />
          <managementService endpoint=""msmq://${server.mtpubsub}/mt_dashboard"" />
        </bus>
        <transports>
          <transport>MassTransit.ServiceBus.MSMQ.MsmqEndpoint, MassTransit.ServiceBus.MSMQ</transport>
        </transports>
      </facility>-->
    </facilities>

    <components>
      <!-- Mass Transit -->
      <component id=""MT.SubscriptionEndPoint""
                 lifestyle=""singleton""
                 service=""MassTransit.ServiceBus.IEndpoint, MassTransit.ServiceBus""
                 type=""MassTransit.ServiceBus.MSMQ.MsmqEndpoint, MassTransit.ServiceBus.MSMQ""
                 >
        <parameters>
          <uriString>msmq://${server.mtpubsub}/${mtpubsub.queue.name}</uriString>
        </parameters>
      </component>

      <component id=""MT.ServiceBus""
                 service=""MassTransit.ServiceBus.IServiceBus, MassTransit.ServiceBus""
                 type=""MassTransit.ServiceBus.ServiceBus, MassTransit.ServiceBus""
                 >
        <parameters>
          <endpointToListenOn>${MT.ListenEndpoint}</endpointToListenOn>
          <subscriptionCache>${MT.LocalSubscriptionCache}</subscriptionCache>
        </parameters>
      </component>

      <component id=""MT.ListenEndpoint""
                 service=""MassTransit.ServiceBus.IEndpoint, MassTransit.ServiceBus""
                 type=""MassTransit.ServiceBus.MSMQ.MsmqEndpoint, MassTransit.ServiceBus.MSMQ""
                 lifestyle=""singleton""
                 >
        <parameters>
          <uriString>msmq://${server.queue}/${b.queue.name}</uriString>
        </parameters>
      </component>

      <component id=""MT.ObjectBuilder""
					       service=""MassTransit.ServiceBus.IObjectBuilder, MassTransit.ServiceBus""
					       type=""MassTransit.WindsorIntegration.WindsorObjectBuilder, MassTransit.WindsorIntegration""
                 />

      <component id=""MT.LocalSubscriptionCache""
					       service=""MassTransit.ServiceBus.Subscriptions.ISubscriptionCache, MassTransit.ServiceBus""
					       type=""MassTransit.ServiceBus.Subscriptions.LocalSubscriptionCache, MassTransit.ServiceBus""
                 >
      </component>

      <component id=""MT.SubscriptionClient""
					       lifestyle=""Singleton""
					       type=""MassTransit.ServiceBus.Subscriptions.SubscriptionClient, MassTransit.ServiceBus""
                 >
        <parameters>
          <serviceBus>${MT.ServiceBus}</serviceBus>
          <cache>${MT.LocalSubscriptionCache}</cache>
          <subscriptionServiceEndpoint>${MT.SubscriptionEndPoint}</subscriptionServiceEndpoint>
        </parameters>
      </component>

      <!-- common -->
      <component id=""TriggerFileWatcher""
                 service=""AppDev.Shared.FileWatch.IFileWatcher, AppDev.Shared""
                 type=""AppDev.Shared.FileWatch.FileWatcher, AppDev.Shared""
                 >
        <parameters>
          <filePath>${dirs.files.toplevel}\BIntegration\TriggerWatch\BIntegrationTrigger.txt</filePath>
          <sleepIntervalInSeconds>${b.filewatch.interval}</sleepIntervalInSeconds>
        </parameters>
      </component>

      <component id=""DateService""
                 service=""AppDev.BIntegration.Infrastructure.IDateService, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.CDateService, AppDev.BIntegration.Infrastructure""
                 />

      <component id=""ApplicationInformationService""
                 service=""AppDev.BIntegration.Infrastructure.Application.IApplicationInformationService, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.Application.ApplicationInformationService, AppDev.BIntegration.Infrastructure""
                 />

      <!-- Integration Runner-->
      <component id=""IntegrationRunner""
                 service=""AppDev.BIntegration.Infrastructure.IIntegrationRunner, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.IntegrationRunner, AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <typeRunners>
            <array>
              <value>${BondTradeTypeRunner}</value>
              <value>${SwapTypeRunner}</value>
              <value>${DiscountNoteTypeRunner}</value>
            </array>
          </typeRunners>
        </parameters>
      </component>

      <!-- Filters -->
      <component id=""NullFilter""
                 service=""AppDev.BIntegration.Infrastructure.Filters.IFilter, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Filters.NullFilter, AppDev.BIntegration""
                 />

      <component id=""TerminationDateFilter""
                 service=""AppDev.BIntegration.Infrastructure.Filters.IFilter, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Filters.TerminationDateFilter, AppDev.BIntegration""
                 >
        <parameters>
          <daysAfterTermination>${c.terminationdatefilter.days}</daysAfterTermination>
        </parameters>
      </component>

      <component id=""MaturityDateFilter""
                 service=""AppDev.BIntegration.Infrastructure.Filters.IFilter, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Filters.MaturityDateFilter, AppDev.BIntegration""
                 />

      <component id=""SettleDateFilter""
                 service=""AppDev.BIntegration.Infrastructure.Filters.IFilter, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Filters.SettleDateFilter, AppDev.BIntegration""
                 >
        <parameters>
          <daysAfterSettle>${b.a.nonmerged.settledatefilter.days}</daysAfterSettle>
        </parameters>
      </component>

      <!-- Other -->
      <component id=""CConfig""
                 service=""AppDev.BIntegration.Infrastructure.Config.IConfig, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.Config.Config, AppDev.BIntegration.Infrastructure""
                 />

      <!-- B Repository -->
      <component id=""BRepository""
                service=""AppDev.BIntegration.Infrastructure.Repositories.IRepository, AppDev.BIntegration.Infrastructure""
                type=""AppDev.BIntegration.Infrastructure.Repositories.Repository, AppDev.BIntegration.Infrastructure""
                />

      <component id=""ARepository""
                 service=""AppDev.BIntegration.Infrastructure.Repositories.IRepository, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.AIntegration.Repositories.ARepository, AppDev.AIntegration""
                 />

      <!-- Bond Trades -->
      <component id=""BondTradeTypeRunner""
                 service=""AppDev.BIntegration.Infrastructure.ITypeRunner, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.TypeRunner`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]], AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <publisher>${BondPublisher}</publisher>
          <filters>
            <array>
              <value>${b.filter.terminationdate}</value>
            </array>
          </filters>
        </parameters>
      </component>

      <component id=""BondPublisher""
                 service=""AppDev.Shared.Integration.Messaging.IPublisher`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]], AppDev.Shared.Integration""
                 type=""AppDev.Shared.Integration.Messaging.MessagePublisher`2[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration],[AppDev.Shared.Messages.Debt.BondMessage,AppDev.Shared.Messages.Debt]],AppDev.Shared.Integration""
                 >
      </component>

      <component id=""BondMapper""
                 service=""AppDev.Shared.Integration.IBatchMapper`2[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration],[AppDev.Shared.Messages.Debt.BondMessage, AppDev.Shared.Messages.Debt]], AppDev.Shared.Integration""
                 type=""AppDev.BIntegration.Mapping.BBondMapper, AppDev.BIntegration""
                 />

      <component id=""BBondServiceManager""
                 service=""AppDev.BIntegration.Infrastructure.IServiceManager`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.BBondServiceManager, AppDev.BIntegration""
                 >
        <parameters>
          <recordsMerger>${BBondMergerValidator}</recordsMerger>
        </parameters>
      </component>

      <component id=""BBondMerger""
                 service=""AppDev.BIntegration.Infrastructure.Combiners.IRecordsMerger`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Combiners.BBondMerger, AppDev.BIntegration""
                 >
        <parameters>
          <filterForANonMergedRecords>${b.merger.a.filter}</filterForANonMergedRecords>
          <firstRoundComparisons>${BBondFirstRoundComparisonList}</firstRoundComparisons>
          <secondRoundComparisons>${BBondSecondRoundComparisonList}</secondRoundComparisons>
          <thirdRoundComparisons>${BBondThirdRoundComparisonList}</thirdRoundComparisons>
        </parameters>
      </component>

      <component id=""BBondMerger-Actual""
                 type=""AppDev.BIntegration.Combiners.BBondMerger, AppDev.BIntegration""
                 >
        <parameters>
          <filterForANonMergedRecords>${b.merger.a.filter}</filterForANonMergedRecords>
          <firstRoundComparisons>${BBondFirstRoundComparisonList}</firstRoundComparisons>
          <secondRoundComparisons>${BBondSecondRoundComparisonList}</secondRoundComparisons>
          <thirdRoundComparisons>${BBondThirdRoundComparisonList}</thirdRoundComparisons>
        </parameters>
      </component>

      <component id=""BBondMergerValidator""
                 service=""AppDev.BIntegration.Infrastructure.Combiners.IRecordsMerger`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Combiners.BBondMergerValidation, AppDev.BIntegration""
                 >
        <parameters>
          <fundingBondMerger>${BBondMerger-Actual}</fundingBondMerger>
          <maturityFilter>${b.filter.maturitydate}</maturityFilter>
          <terminationFilter>${b.filter.terminationdate}</terminationFilter>
        </parameters>
      </component>

      <!-- A Bond Comparisons -->
      <component id=""BBondFirstRoundComparisonList""
                 type=""AppDev.BIntegration.Infrastructure.Comparisons.ComparisonsList`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <comparisons>
            <array>
              <value>${BSeriesNumberComparison}</value>
              <value>${BSettleDateComparison}</value>
              <value>${BCounterpartyComparison}</value>
            </array>
          </comparisons>
        </parameters>
      </component>

      <component id=""BBondSecondRoundComparisonList""
                 type=""AppDev.BIntegration.Infrastructure.Comparisons.ComparisonsList`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <comparisons>
            <array>
              <value>${BSeriesNumberComparison}</value>
              <value>${BParAmountComparison}</value>
            </array>
          </comparisons>
        </parameters>
      </component>

      <component id=""BBondThirdRoundComparisonList""
               type=""AppDev.BIntegration.Infrastructure.Comparisons.ComparisonsList`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <comparisons>
            <array>
              <value>${BSeriesNumberComparison}</value>
              <value>${BReceiveCouponPercentComparison}</value>
            </array>
          </comparisons>
        </parameters>
      </component>

      <component id=""BCusipComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Comparisons.CusipComparison, AppDev.BIntegration""
                 />

      <component id=""BSettleDateComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Comparisons.SettleDateComparison, AppDev.BIntegration""
                 />

      <component id=""BCounterpartyComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Comparisons.CounterpartyComparison, AppDev.BIntegration""
                 />

      <component id=""BParAmountComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Comparisons.ParAmountComparison, AppDev.BIntegration""
                 />

      <component id=""BReceiveCouponPercentComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Comparisons.CouponPercentComparison, AppDev.BIntegration""
                 />

      <component id=""BSeriesNumberComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.BIntegration.Domain.BBond, AppDev.BIntegration]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Comparisons.SeriesNumberComparison, AppDev.BIntegration""
                 />


      <!-- A Bond stuff -->
      <component id=""ABondServiceManager""
                 service=""AppDev.BIntegration.Infrastructure.IServiceManager`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.AIntegration.OoFBondServiceManager, AppDev.AIntegration""
                 >
      </component>
      <!-- BondService Options: StubBondService or OoFDatabaseBondService-->
      <component id=""ABondService""
                 service=""AppDev.AIntegration.Services.IBondService`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]], AppDev.AIntegration""
          	 type=""AppDev.AIntegration.Services.${a.b.bondservice.type},AppDev.AIntegration""
                 >
        <parameters>
          <repository>${ARepository}</repository>
        </parameters>
      </component>

      <component id=""ABondTradeCombiner""
                 service=""AppDev.BIntegration.Infrastructure.Combiners.IRecordCombiner`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.AIntegration.Infrastructure.Combiners.OoFBondTradeDetailCombiner, AppDev.AIntegration""
                 >
      </component>

      <!-- A Bond Comparisons -->
      <component id=""ABondTradeComparisonList""
                 type=""AppDev.BIntegration.Infrastructure.Comparisons.ComparisonsList`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]],AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <comparisons>
            <array>
              <value>${ACusipComparison}</value>
              <value>${ASettleDateComparison}</value>
              <value>${ACounterpartyComparison}</value>
            </array>
          </comparisons>
        </parameters>
      </component>

      <component id=""ACusipComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.AIntegration.Infrastructure.Comparisons.CusipComparison, AppDev.AIntegration""
                 />
      <component id=""ASettleDateComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.AIntegration.Infrastructure.Comparisons.SettleDateComparison, AppDev.AIntegration""
                 />
      <component id=""ACounterpartyComparison""
                 service=""AppDev.BIntegration.Infrastructure.Comparisons.IComparisonSpecification`1[[AppDev.AIntegration.WebServiceProxy.BondTradeEnquiryResponseBondTradeDetail, A.WebServiceProxy]],AppDev.BIntegration.Infrastructure""
                 type=""AppDev.AIntegration.Infrastructure.Comparisons.CounterpartyComparison, AppDev.AIntegration""
                 />

      <component id=""ASwapCounterpartyMapper""
                 service=""AppDev.AIntegration.Mapping.ICounterpartyMapper, AppDev.AIntegration""
                 type=""AppDev.AIntegration.Mapping.CounterpartyMapper, AppDev.AIntegration""
                 >
        <parameters>
          <repository>${ARepository}</repository>
        </parameters>
      </component>


      <!-- CBond stuff -->
      <component id=""CBondServiceManager""
                 service=""AppDev.BIntegration.Infrastructure.IServiceManager`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.CIntegration.CServiceManager`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.CIntegration""
                 >
      </component>

      <!-- BondService Options: StubBondService or CService`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]]-->
      <component id=""CBondService""
                 service=""AppDev.BIntegration.Infrastructure.Services.IBService`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
          	 type=""AppDev.CIntegration.Services.${c.bondservice.type},AppDev.CIntegration""
                 />

      <component id=""CBondsCombiner""
                 service=""AppDev.BIntegration.Infrastructure.Combiners.IRecordCombiner`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.CIntegration.Combiners.CBondCombiner, AppDev.CIntegration""
                 />

      <component id=""CBondCorrector""
                 service=""AppDev.CIntegration.Correctors.ICorrector`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.CIntegration""
                 type=""AppDev.CIntegration.Correctors.CSeriesNumberCorrector`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.CIntegration""
                 />

      <component id=""CBondFileHelpers""
                 service=""AppDev.BIntegration.Infrastructure.IFileHelpersAccessor`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.FileHelpersAccessor`1[[AppDev.CIntegration.CBond, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 />

      <!-- Swaps -->

      <component id=""SwapTypeRunner""
                 service=""AppDev.BIntegration.Infrastructure.ITypeRunner, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.TypeRunner`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <publisher>${SwapPublisher}</publisher>
          <filters>
            <array>
              <value>${b.filter.terminationdate}</value>
            </array>
          </filters>
        </parameters>
      </component>

      <component id=""SwapPublisher""
                 service=""AppDev.Shared.Integration.Messaging.IPublisher`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.Shared.Integration""
                 type=""AppDev.Shared.Integration.Messaging.MessagePublisher`2[[AppDev.CIntegration.CSwap, AppDev.CIntegration],[AppDev.Shared.Messages.Derivatives.SwapMessage,AppDev.Shared.Messages.Derivatives]],AppDev.Shared.Integration""
                 />

      <component id=""SwapMapper""
                 service=""AppDev.Shared.Integration.IBatchMapper`2[[AppDev.CIntegration.CSwap, AppDev.CIntegration],[AppDev.Shared.Messages.Derivatives.SwapMessage, AppDev.Shared.Messages.Derivatives]], AppDev.Shared.Integration""
                 type=""AppDev.BIntegration.Mapping.CSwapMapper, AppDev.BIntegration""
                 />

      <component id=""SwapServiceManager""
                 service=""AppDev.BIntegration.Infrastructure.IServiceManager`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.CIntegration.CServiceManager`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.CIntegration""
                 />

      <!-- SwapService Options: StubSwapService or CService`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]]-->
      <component id=""SwapService""
                 service=""AppDev.BIntegration.Infrastructure.Services.IBService`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
          	 type=""AppDev.CIntegration.Services.${c.swapservice.type},AppDev.CIntegration""
                 />

      <component id=""SwapsCombiner""
                 service=""AppDev.BIntegration.Infrastructure.Combiners.IRecordCombiner`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.Combiners.NullCombiner`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 />

      <component id=""SwapsCorrector""
                 service=""AppDev.CIntegration.Correctors.ICorrector`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.CIntegration""
                 type=""AppDev.CIntegration.Correctors.NullCorrector`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.CIntegration""
                 />

      <component id=""SwapFileHelpers""
                 service=""AppDev.BIntegration.Infrastructure.IFileHelpersAccessor`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.FileHelpersAccessor`1[[AppDev.CIntegration.CSwap, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 />

      <!-- Discount Notes -->

      <component id=""DiscountNoteTypeRunner""
                 service=""AppDev.BIntegration.Infrastructure.ITypeRunner, AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.TypeRunner`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 >
        <parameters>
          <publisher>${DiscountNotePublisher}</publisher>
          <filters>
            <array>
              <value>${b.filter.terminationdate}</value>
            </array>
          </filters>
        </parameters>
      </component>

      <component id=""DiscountNotePublisher""
                 service=""AppDev.Shared.Integration.Messaging.IPublisher`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.Shared.Integration""
                 type=""AppDev.Shared.Integration.Messaging.MessagePublisher`2[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration],[AppDev.Shared.Messages.Debt.DiscountNoteMessage,AppDev.Shared.Messages.Debt]],AppDev.Shared.Integration""
                 />

      <component id=""DiscountNoteMapper""
                 service=""AppDev.Shared.Integration.IBatchMapper`2[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration],[AppDev.Shared.Messages.Debt.DiscountNoteMessage, AppDev.Shared.Messages.Debt]], AppDev.Shared.Integration""
                 type=""AppDev.BIntegration.Mapping.CDiscountNoteMapper, AppDev.BIntegration""
                 />

      <component id=""DiscountNoteServiceManager""
                 service=""AppDev.BIntegration.Infrastructure.IServiceManager`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.CIntegration.CServiceManager`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.CIntegration""
                 />

      <!-- DiscountNoteService Options: StubDiscountNoteService or CService`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]] -->
      <component id=""DiscountNoteService""
                 service=""AppDev.BIntegration.Infrastructure.Services.IBService`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
          	 type=""AppDev.CIntegration.Services.${c.discountnoteservice.type},AppDev.CIntegration""
                 />

      <component id=""DiscountNoteCombiner""
                 service=""AppDev.BIntegration.Infrastructure.Combiners.IRecordCombiner`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.Combiners.NullCombiner`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                  />

      <component id=""DiscountNoteCorrector""
                 service=""AppDev.CIntegration.Correctors.ICorrector`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.CIntegration""
                 type=""AppDev.CIntegration.Correctors.NullCorrector`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.CIntegration""
                 />

      <component id=""DiscountNoteFileHelpers""
                 service=""AppDev.BIntegration.Infrastructure.IFileHelpersAccessor`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 type=""AppDev.BIntegration.Infrastructure.FileHelpersAccessor`1[[AppDev.CIntegration.CDiscountNote, AppDev.CIntegration]], AppDev.BIntegration.Infrastructure""
                 />

    </components>
  </castle>

  <log4net>
    <!-- Appender Config Examples: http://logging.apache.org/log4net/release/config-examples.html -->
    <appender name=""Console"" type=""log4net.Appender.ConsoleAppender"">
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%message%newline""/>
        <!--<conversionPattern value=""%-4timestamp [%thread] %-5level %logger %ndc - %message%newline"" />-->
      </layout>
    </appender>

    <appender name=""RollingFile"" type=""log4net.Appender.RollingFileAppender"">
      <file value="".\logs\AppDev.BIntegration.Host.log""/>
      <appendToFile value=""true""/>
      <rollingStyle value=""Size""/>
      <maxSizeRollBackups value=""10""/>
      <maximumFileSize value=""1MB""/>
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date [%level] - %message%newline""/>
      </layout>
    </appender>

    <appender name=""EventLogAppender"" type=""log4net.Appender.EventLogAppender"">
      <LogName value=""Applications""/>
      <ApplicationName value=""Applications""/>
      <threshold value=""ERROR""/>
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date [%thread] %-5level %logger - %message%newline%newline%""/>
      </layout>
    </appender>

    <appender name=""ErrorSmtpAppender"" type=""log4net.Appender.SmtpAppender"">
      <to value=""${email.errors.to}"" />
      <from value=""B.Integration@fhlbtopeka.com""/>
      <subject value=""B Integration Host Errors - ${environment}"" />
      <smtpHost value=""mail.fhlbtopeka.com""/>
      <bufferSize value=""2""/>
      <lossy value=""true""/>
      <evaluator type=""log4net.Core.LevelEvaluator"">
        <threshold value=""ERROR""/>
      </evaluator>
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%newline%date [%thread] %-5level %logger - %message%newline""/>
        <!--<conversionPattern value=""%newline%date [%thread] %-5level %logger [%property{NDC}] - %message%newline"" />-->
      </layout>
    </appender>

    <!-- Custom appender for NHibernate's SQL -->
    <appender name=""NH.SQL"" type=""log4net.Appender.FileAppender"">
      <file value="".\logs\nhibernate.sql.log""/>
      <appendToFile value=""false""/>
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date [%thread] %-5level %logger [%property{NDC}] - %message%newline""/>
      </layout>
    </appender>

    <!-- Custom appender for MassTransit's Messages -->
    <appender name=""MT.Log"" type=""log4net.Appender.FileAppender"">
      <file value="".\logs\masstransit.log""/>
      <appendToFile value=""false""/>
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date [%thread] %-5level %logger [%property{NDC}] - %message%newline""/>
      </layout>
    </appender>

    <appender name=""MT.Messages"" type=""log4net.Appender.FileAppender"">
      <file value="".\logs\masstransit.messages.log""/>
      <appendToFile value=""false""/>
      <layout type=""log4net.Layout.PatternLayout"">
        <conversionPattern value=""%date [%thread] %-5level %logger [%property{NDC}] - %message%newline""/>
      </layout>
    </appender>

    <root>
      <level value=""${log.level}"" />
      <appender-ref ref=""Console""/>
    </root>

    <!-- Standard NHibernate Logging Settings -->
    <logger name=""NHibernate"">
      <level value=""ERROR""/>
    </logger>

    <logger name=""NHibernate.SQL"">
      <level value=""ERROR""/>
      <appender-ref ref=""NH.SQL""/>
    </logger>

    <!-- Standard MassTransit Logging Settings -->
    <logger name=""MassTransit"">
      <level value=""ERROR""/>
      <appender-ref ref=""MT.Log""/>
    </logger>

    <logger name=""MassTransit.Messages"">
      <level value=""ERROR""/>
      <appender-ref ref=""MT.Messages""/>
    </logger>

    <!-- Standard Bank Settings -->
    <logger name=""AppDev"">
      <level value=""${log.level}"" />
      <appender-ref ref=""RollingFile""/>
      <appender-ref ref=""ErrorSmtpAppender""/>
      <appender-ref ref=""EventLogAppender""/>
    </logger>

  </log4net>

</configuration>";
    }
}