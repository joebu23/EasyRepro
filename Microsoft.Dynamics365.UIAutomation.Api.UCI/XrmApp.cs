// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT license.

using Microsoft.Dynamics365.UIAutomation.Browser;
using OpenQA.Selenium;
using OpenQA.Selenium.Interactions;
using System;
using System.Collections.Generic;
using System.Web;

namespace Microsoft.Dynamics365.UIAutomation.Api.UCI
{
    public class XrmApp : IDisposable
    {
        internal WebClient _client;
        internal IWebDriver _driver => _client._driver;

        public List<ICommandResult> CommandResults => _client.CommandResults;

        public XrmApp(WebClient client)
        {
            _client = client;
        }

        public OnlineLogin OnlineLogin => this.GetElement<OnlineLogin>(_client);
        public Navigation Navigation => this.GetElement<Navigation>(_client);
        public CommandBar CommandBar => this.GetElement<CommandBar>(_client);
        public Grid Grid => this.GetElement<Grid>(_client);
        public Entity Entity => this.GetElement<Entity>(_client);
        public Dialogs Dialogs => this.GetElement<Dialogs>(_client);
        public Timeline Timeline => this.GetElement<Timeline>(_client);
        public BusinessProcessFlow BusinessProcessFlow => this.GetElement<BusinessProcessFlow>(_client);
        public Dashboard Dashboard => this.GetElement<Dashboard>(_client);
        public RelatedGrid RelatedGrid => this.GetElement<RelatedGrid>(_client);

        public GlobalSearch GlobalSearch => this.GetElement<GlobalSearch>(_client);
		public QuickCreate QuickCreate => this.GetElement<QuickCreate>(_client);
        public Lookup Lookup => this.GetElement<Lookup>(_client);
        public Telemetry Telemetry => this.GetElement<Telemetry>(_client);

        public T GetElement<T>(WebClient client)
            where T : Element
        {
            return (T)Activator.CreateInstance(typeof(T), new object[] { client });
        }

        public void ThinkTime(int milliseconds)
        {
            _client.ThinkTime(milliseconds);
        }
        public void ThinkTime(TimeSpan timespan)
        {
            _client.ThinkTime((int)timespan.TotalMilliseconds);
        }

        /// <summary>
        /// Replicates a click on the first element in a standard view table - NOTE: Requires that the first column be the anchor tag with a link to the main view form
        /// </summary>
        /// <param name="cssSelector">String selector to click on</param>
        public void ClickFirstElementBySelector(string cssSelector)
        {
            Actions actions = new Actions(this._driver);
            IWebElement element = this._driver.FindElement(By.CssSelector(cssSelector));
            actions.Click(element);
            actions.Build();
            actions.Perform();
            this.ThinkTime(2000);
        }

        /// <summary>
        /// Clicks the 'Discard Changes' button on modal when trying to cancel out of saving things
        /// </summary>
        public void ClickDiscardChangesButton()
        {
            this.ThinkTime(2000);
            Actions actions = new Actions(this._driver);
            IWebElement element = this._driver.FindElement(By.Id("cancelButtonTextName"));
            actions.Click(element);
            actions.Build();
            actions.Perform();
            this.ThinkTime(2000);
        }

        public string GetQueryParameter(string paramName)
        {
            var url = this._driver.Url;
            return HttpUtility.ParseQueryString(url).Get(paramName);
        }

        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
