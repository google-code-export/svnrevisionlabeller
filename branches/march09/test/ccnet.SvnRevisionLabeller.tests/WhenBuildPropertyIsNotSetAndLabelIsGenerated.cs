using ccnet.labeller.tests;
using NUnit.Framework;
using NUnit.Framework.SyntaxHelpers;
using Rhino.Mocks;
using ThoughtWorks.CruiseControl.Core;

namespace ccnet.labeller.tests
{
	[TestFixture]
	public class WhenBuildPropertyIsNotSetAndLabelIsGenerated : Specification
	{
		protected override void Arrange()
		{
			_previousResult = Mockery.DynamicMock<IIntegrationResult>();
			Expect.Call(_previousResult.Label).Return("1.0.100.0");

			_labeller = new SvnRevisionLabellerStub();
			_labeller.SetRevision(101);
		}

		protected override void Act()
		{
			_label = _labeller.Generate(_previousResult);
		}

		[Test]
		public void BuildNumberIsSetToTheCurrentSvnRevisionNumber()
		{
			Assert.That(_label, Is.EqualTo("1.0.101.0"));
		}

		private SvnRevisionLabellerStub _labeller;
		private IIntegrationResult _previousResult;
		private string _label;
	}
}