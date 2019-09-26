using NUnit.Framework;
using SmeData.SharedModels.Document;
using SmeData.WebApi.Data.Eucases;
using SmeData.WebApi.Services;
using SmeData.WebApi.Services.Documents;
using SmeData.WebApi.Tests;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Tests
{
    public class TestDocumentsService
    {
        private IDocumentsService docsService;

        [SetUp]
        public void Setup()
        {
            //this.RestoreDatabase();
            var cs = @"Server=localhost;Port=5432;Database=sme_data;User Id=postgres;Password=L1m0n@d@; Timeout=300";
            var factory = new EucasesContextFactory(new EuCasesContextFactorySettings(cs));
            var pathsProvider = new PathsProvider()
            {
                BasePath = AppDomain.CurrentDomain.BaseDirectory
            };
            this.docsService = new DocumentsService(factory, pathsProvider);
        }

        private void RestoreDatabase()
        {
            var dbDumpConfigStr = File.ReadAllText(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, @".\Config\Config.json"));
            var dbConf = DbConfig.LoadFromJson(dbDumpConfigStr);
            new PostGressDumpRestore(dbConf).Restore("sme_data_12072019_163611.pg_dump");
        }

        [Test]
        public void TestGetSmeDocByDocLangId_LegislationDoc()
        {
            var res = this.docsService.GetSmeDocByDocIdentifier("5b93ca5c-7d49-4b30-b70d-9d4204086a74", string.Empty);

            Assert.AreEqual(59, res.Items.Count);

            Assert.AreEqual(6210537, res.Meta.DocLangId);
            Assert.AreEqual("32007D0533", res.Meta.DocNumber);
            Assert.AreEqual("5b93ca5c-7d49-4b30-b70d-9d4204086a74", res.Meta.Idenitifier);
            Assert.AreEqual("Council Decision 2007/533/JHA of 12 June 2007 on the establishment, operation and use of the second generation Schengen Information System (SIS II)", res.Meta.Title);
            Assert.AreEqual(null, res.Meta.PublicationDate);
            Assert.AreEqual(new DateTime(2007, 6, 12), res.Meta.ActDate);
            Assert.AreEqual(new DateTime(2019, 7, 5), res.Meta.LastChangeDate);
            Assert.AreEqual("EU", res.Meta.Country);
            Assert.AreEqual("en", res.Meta.ShortLang);
            Assert.AreEqual("eng", res.Meta.Language);
            Assert.AreEqual(4, res.Meta.LangId);
        }

        [Test]
        public void TestGetSmeDocByDocLangId_CaseLawDoc()
        {
            var res = this.docsService.GetSmeDocByDocIdentifier("dd1841c7-2482-4c44-a512-ec9276870de1", null);

            Assert.AreEqual(1, res.Items.Count);
            Assert.AreEqual(@"<div class=""d- d-maincontent d-mainBody"">
    <div class=""d- d-blockContainer d-blockContainer"">
      <p class=""d- d-inline d-p"">Adopted on 13 December 2016</p>
      <p class=""d- d-inline d-p"">This Working Party was set up under <a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=31995L0046&amp;ToPar=Art29"" id=""ref_1"">Article 29 of Directive 95/46/EC</a>. It is an independent European advisory body on data protection and privacy. Its tasks are described in <a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=31995L0046&amp;ToPar=Art30"" id=""ref_2"">Article 30 of Directive 95/46/EC</a> and <a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=32002L0058&amp;ToPar=Art15"" id=""ref_3"">Article 15 of Directive 2002/58/EC</a>.</p>
      <p class=""d- d-inline d-p"">The secretariat is provided by Directorate C (Fundamental rights and rule of law) of the European Commission, Directorate General Justice and Consumers, B-1049 Brussels, Belgium, Office No MO59 02/27</p>
      <p class=""d- d-inline d-p"">Website:<a class=""d- d-inline d-ref"" target=""_blank"" href=""http://ec.europa.eu/justice/data-protection/index_en.htm"" id=""ref_1""> http://ec.europa.eu/justice/data-protection/index en.htm</a></p>
      <p class=""d- d-inline d-p"">
        <strong class=""d- d-inline d-b"">EU-US PRIVACY SHIELD F.A.Q. FOR EUROPEAN INDIVIDUALS<a class=""d- d-inline d-ref"" href=""#_ftn1"" id=""ref_1""><strong class=""d- d-inline d-b"">[1]</strong></a></strong>
      </p>
      <p class=""d- d-inline d-p"">What is the Privacy Shield?</p>
      <p class=""d- d-inline d-p"">The <u class=""d- d-inline d-u""><a class=""d- d-inline d-ref"" target=""_blank"" href=""http://ec.europa.eu/justice/data-protection/international-transfers/eu-us-privacy-shield/index_en.htm"" id=""ref_1"">Privacy Shield</a></u><a class=""d- d-inline d-ref"" href=""#_ftn2"" id=""ref_1""><u class=""d- d-inline d-u""><u class=""d- d-inline d-u"">[2]</u></u></a> is a self-certification mechanism for US based companies. This framework has been recognized by the European Commission as providing an adequate level of protection for personal data transferred from an EU entity to US based companies and thus as an element for offering legal guarantees for such data transfers.</p>
      <p class=""d- d-inline d-p"">The EU-US Privacy Shield mechanism is in full effect since the 1<sup class=""d- d-inline d-sup"">st</sup> of August 2016.</p>
      <p class=""d- d-inline d-p"">The Privacy Shield applies to any type of personal data transferred from an EU entity to the US including commercial, health or human resource related data, as long as the recipient US company has self-certified to the Framework.</p>
      <p class=""d- d-inline d-p"">How do I benefit from the Privacy Shield?</p>
      <p class=""d- d-inline d-p"">The Privacy Shield relies on commitments taken by US companies to respect the principles, rules and obligations laid out by the Privacy Shield framework.</p>
      <p class=""d- d-inline d-p"">This framework grants you a certain number of rights when your personal data have been transferred from an EU entity to the US. Notably, you have the right to be informed of such transfer and to exercise your rights of access, for example of correction and of deletion of your personal data transferred<a class=""d- d-inline d-ref"" href=""#_ftn3"" id=""ref_1""><sup class=""d- d-inline d-sup""><sup class=""d- d-inline d-sup"">[3]</sup></sup></a>. You can verify whether a US based company has certified by checking the online Privacy Shield list, available here:<a class=""d- d-inline d-ref"" target=""_blank"" href=""http://www.privacyshield.gov/"" id=""ref_2""> www.privacyshield.gov.</a></p>
      <p class=""d- d-inline d-p"">It is encouraged to address possible queries regarding the processing of your data to the US company, first.</p>
      <p class=""d- d-inline d-p"">If your concern has not been resolved by the Privacy Shield company or you have reasons to not address it directly, your national data protection authority will stand ready to help you to resolve the matter.</p>
      <p class=""d- d-inline d-p"">How do I lodge a complaint?</p>
      <p class=""d- d-inline d-p"">In case you think that the US Privacy-Shield company has violated its obligations stemming from the EU-US Privacy Shield Framework or has violated the rights entitled to you under the Privacy shield principles, you can complain about it.</p>
      <p class=""d- d-inline d-p"">If you want to lodge a complaint regarding an US Privacy Shield certified company, or a company that claims to have been certified, please use the common complaint form available here (soon available) or contact your national DPA<a class=""d- d-inline d-ref"" href=""#_ftn4"" id=""ref_1""><sup class=""d- d-inline d-sup""><sup class=""d- d-inline d-sup"">[4]</sup></sup></a>. Please provide your national DPA with as many details on the matter as possible, enabling your DPA to handle your complaint in the best way.</p>
      <p class=""d- d-inline d-p"">An informal panel of EU DPAs will be set up in order to handle complaints concerning human resources personal data transferred from an EU entity to an US Privacy Shield company in the context of employment relationship, or when the US recipient company has voluntarily chosen to commit to cooperate with the EU DPAs.</p>
      <p class=""d- d-inline d-p"">The informal panel of EU DPAs will launch an investigation during which both parties will have the possibility to express their views. If necessary in order to resolve the case, the informal panel can issue an ""advice"" which is a binding decision that the US Privacy Shield company will have to comply with.</p>
      <p class=""d- d-inline d-p"">Where the informal panel of EU DPAs is not competent, EU DPAs have the possibility to refer the complaint to US authorities (notably, the FTC committed to give priority consideration to those referrals and the DoC has a clear deadline to act on complaints). In any cases, depending on the circumstances of the case, the competent national DPA may also directly exercise its powers (such as prohibition or suspension of data transfers) toward the EU data exporter.</p>
      <p class=""d- d-inline d-p"">For getting more information about the possibility to lodge a complaint, you may ask further information to your national data protection authority.</p>
      <p class=""d- d-inline d-p"">The data protection authorities are currently developing a common complaint form that may be used by EU individuals to submit a complaint. The complaint form will be provided as soon as possible. The complaint form will be optional, so you can lodge a complaint already by contacting your national DPA.</p>
      <p class=""d- d-inline d-p"">Please note that requests relating to access by US public authorities for intelligence activities are subject to another procedure. Please contact your national DPA for more information.</p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-inline d-ref"" href=""#_ftnref1"" id=""ref_1"">
          <sup class=""d- d-inline d-sup"">
            <sup class=""d- d-inline d-sup"">[1]</sup>
          </sup>
        </a> In this context, European individuals means any natural person, regardless of his/her nationality, whose personal data have been transferred to a US company under the EU-US Privacy Shield.</p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-c-footnote d-inline d-ref"" refersto=""#_ftnref2"" id=""_ftn2"">[2]</a> The decision on the adequacy of the EU-U.S. Privacy Shield Framework (""Privacy Shield"") or (""Framework"") was adopted by the European Commission on July 12, 2016. It was designed by the European Commission and the U.S. Department of Commerce to replace the Safe-Harbor-<a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=32000D0520"" id=""ref_2"">Decision 2000/520/EC</a> which were declared invalid by the European Court of Justice in 6 October 2015.</p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-inline d-ref"" href=""#_ftnref3"" id=""ref_1"">
          <sup class=""d- d-inline d-sup"">
            <sup class=""d- d-inline d-sup"">[3]</sup>
          </sup>
        </a> For more detailed information as to the guarantees for the data transferred and as to your rights under the EU-U.S. Privacy Shield, please consult the <u class=""d- d-inline d-u""><a class=""d- d-inline d-ref"" target=""_blank"" href=""http://ec.europa.eu/justice/data-protection/files/eu-us_privacy_shield_guide_en.pdf"" id=""ref_1"">Guide to the EU-US Privacy Shield published by the European Commission.</a></u></p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-inline d-ref"" href=""#_ftnref4"" id=""ref_1"">
          <sup class=""d- d-inline d-sup"">
            <sup class=""d- d-inline d-sup"">[4]</sup>
          </sup>
        </a> Whenever the words ""national data protection authority"", ""EU DPA"" or ""EU handling authority"", this also refers to the EDPS, which will be the EU handling authority in case where your personal data have been transferred to an US Privacy Shield certified company by an EU institution.</p>
    </div>
  </div>", res.Items[0].Text);

            Assert.AreEqual(5780406, res.Meta.DocLangId);
            Assert.AreEqual("30003_GDPE", res.Meta.DocNumber);
            Assert.AreEqual("dd1841c7-2482-4c44-a512-ec9276870de1", res.Meta.Idenitifier);
            Assert.AreEqual("EU-US PRIVACY SHIELD F.A.Q. FOR EUROPEAN INDIVIDUALS", res.Meta.Title);
            Assert.AreEqual(DateTime.MinValue, res.Meta.PublicationDate);
            Assert.AreEqual(DateTime.MinValue, res.Meta.ActDate);
            Assert.AreEqual(new DateTime(2019, 6, 27), res.Meta.LastChangeDate);
            Assert.AreEqual("BG", res.Meta.Country);
            Assert.AreEqual("en", res.Meta.ShortLang);
            Assert.AreEqual("eng", res.Meta.Language);
            Assert.AreEqual(4, res.Meta.LangId);
        }

        [Test]
        public void TestGetSmeDocByDocIdentifier_LegislationDoc()
        {
            var res = this.docsService.GetSmeDocByDocIdentifier("5b93ca5c-7d49-4b30-b70d-9d4204086a74", string.Empty);

            Assert.AreEqual(59, res.Items.Count);

            Assert.AreEqual(6210537, res.Meta.DocLangId);
            Assert.AreEqual("32007D0533", res.Meta.DocNumber);
            Assert.AreEqual("5b93ca5c-7d49-4b30-b70d-9d4204086a74", res.Meta.Idenitifier);
            Assert.AreEqual("Council Decision 2007/533/JHA of 12 June 2007 on the establishment, operation and use of the second generation Schengen Information System (SIS II)", res.Meta.Title);
            Assert.AreEqual(null, res.Meta.PublicationDate);
            Assert.AreEqual(new DateTime(2007, 6, 12), res.Meta.ActDate);
            Assert.AreEqual(new DateTime(2019, 7, 5), res.Meta.LastChangeDate);
            Assert.AreEqual("EU", res.Meta.Country);
            Assert.AreEqual("en", res.Meta.ShortLang);
            Assert.AreEqual("eng", res.Meta.Language);
            Assert.AreEqual(4, res.Meta.LangId);
        }

        [Test]
        public void TestGetSmeDocByDocIdentifier_CaseLawDoc()
        {
            var res = this.docsService.GetSmeDocByDocIdentifier("dd1841c7-2482-4c44-a512-ec9276870de1", string.Empty);

            Assert.AreEqual(1, res.Items.Count);
            Assert.AreEqual(@"<div class=""d- d-maincontent d-mainBody"">
    <div class=""d- d-blockContainer d-blockContainer"">
      <p class=""d- d-inline d-p"">Adopted on 13 December 2016</p>
      <p class=""d- d-inline d-p"">This Working Party was set up under <a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=31995L0046&amp;ToPar=Art29"" id=""ref_1"">Article 29 of Directive 95/46/EC</a>. It is an independent European advisory body on data protection and privacy. Its tasks are described in <a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=31995L0046&amp;ToPar=Art30"" id=""ref_2"">Article 30 of Directive 95/46/EC</a> and <a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=32002L0058&amp;ToPar=Art15"" id=""ref_3"">Article 15 of Directive 2002/58/EC</a>.</p>
      <p class=""d- d-inline d-p"">The secretariat is provided by Directorate C (Fundamental rights and rule of law) of the European Commission, Directorate General Justice and Consumers, B-1049 Brussels, Belgium, Office No MO59 02/27</p>
      <p class=""d- d-inline d-p"">Website:<a class=""d- d-inline d-ref"" target=""_blank"" href=""http://ec.europa.eu/justice/data-protection/index_en.htm"" id=""ref_1""> http://ec.europa.eu/justice/data-protection/index en.htm</a></p>
      <p class=""d- d-inline d-p"">
        <strong class=""d- d-inline d-b"">EU-US PRIVACY SHIELD F.A.Q. FOR EUROPEAN INDIVIDUALS<a class=""d- d-inline d-ref"" href=""#_ftn1"" id=""ref_1""><strong class=""d- d-inline d-b"">[1]</strong></a></strong>
      </p>
      <p class=""d- d-inline d-p"">What is the Privacy Shield?</p>
      <p class=""d- d-inline d-p"">The <u class=""d- d-inline d-u""><a class=""d- d-inline d-ref"" target=""_blank"" href=""http://ec.europa.eu/justice/data-protection/international-transfers/eu-us-privacy-shield/index_en.htm"" id=""ref_1"">Privacy Shield</a></u><a class=""d- d-inline d-ref"" href=""#_ftn2"" id=""ref_1""><u class=""d- d-inline d-u""><u class=""d- d-inline d-u"">[2]</u></u></a> is a self-certification mechanism for US based companies. This framework has been recognized by the European Commission as providing an adequate level of protection for personal data transferred from an EU entity to US based companies and thus as an element for offering legal guarantees for such data transfers.</p>
      <p class=""d- d-inline d-p"">The EU-US Privacy Shield mechanism is in full effect since the 1<sup class=""d- d-inline d-sup"">st</sup> of August 2016.</p>
      <p class=""d- d-inline d-p"">The Privacy Shield applies to any type of personal data transferred from an EU entity to the US including commercial, health or human resource related data, as long as the recipient US company has self-certified to the Framework.</p>
      <p class=""d- d-inline d-p"">How do I benefit from the Privacy Shield?</p>
      <p class=""d- d-inline d-p"">The Privacy Shield relies on commitments taken by US companies to respect the principles, rules and obligations laid out by the Privacy Shield framework.</p>
      <p class=""d- d-inline d-p"">This framework grants you a certain number of rights when your personal data have been transferred from an EU entity to the US. Notably, you have the right to be informed of such transfer and to exercise your rights of access, for example of correction and of deletion of your personal data transferred<a class=""d- d-inline d-ref"" href=""#_ftn3"" id=""ref_1""><sup class=""d- d-inline d-sup""><sup class=""d- d-inline d-sup"">[3]</sup></sup></a>. You can verify whether a US based company has certified by checking the online Privacy Shield list, available here:<a class=""d- d-inline d-ref"" target=""_blank"" href=""http://www.privacyshield.gov/"" id=""ref_2""> www.privacyshield.gov.</a></p>
      <p class=""d- d-inline d-p"">It is encouraged to address possible queries regarding the processing of your data to the US company, first.</p>
      <p class=""d- d-inline d-p"">If your concern has not been resolved by the Privacy Shield company or you have reasons to not address it directly, your national data protection authority will stand ready to help you to resolve the matter.</p>
      <p class=""d- d-inline d-p"">How do I lodge a complaint?</p>
      <p class=""d- d-inline d-p"">In case you think that the US Privacy-Shield company has violated its obligations stemming from the EU-US Privacy Shield Framework or has violated the rights entitled to you under the Privacy shield principles, you can complain about it.</p>
      <p class=""d- d-inline d-p"">If you want to lodge a complaint regarding an US Privacy Shield certified company, or a company that claims to have been certified, please use the common complaint form available here (soon available) or contact your national DPA<a class=""d- d-inline d-ref"" href=""#_ftn4"" id=""ref_1""><sup class=""d- d-inline d-sup""><sup class=""d- d-inline d-sup"">[4]</sup></sup></a>. Please provide your national DPA with as many details on the matter as possible, enabling your DPA to handle your complaint in the best way.</p>
      <p class=""d- d-inline d-p"">An informal panel of EU DPAs will be set up in order to handle complaints concerning human resources personal data transferred from an EU entity to an US Privacy Shield company in the context of employment relationship, or when the US recipient company has voluntarily chosen to commit to cooperate with the EU DPAs.</p>
      <p class=""d- d-inline d-p"">The informal panel of EU DPAs will launch an investigation during which both parties will have the possibility to express their views. If necessary in order to resolve the case, the informal panel can issue an ""advice"" which is a binding decision that the US Privacy Shield company will have to comply with.</p>
      <p class=""d- d-inline d-p"">Where the informal panel of EU DPAs is not competent, EU DPAs have the possibility to refer the complaint to US authorities (notably, the FTC committed to give priority consideration to those referrals and the DoC has a clear deadline to act on complaints). In any cases, depending on the circumstances of the case, the competent national DPA may also directly exercise its powers (such as prohibition or suspension of data transfers) toward the EU data exporter.</p>
      <p class=""d- d-inline d-p"">For getting more information about the possibility to lodge a complaint, you may ask further information to your national data protection authority.</p>
      <p class=""d- d-inline d-p"">The data protection authorities are currently developing a common complaint form that may be used by EU individuals to submit a complaint. The complaint form will be provided as soon as possible. The complaint form will be optional, so you can lodge a complaint already by contacting your national DPA.</p>
      <p class=""d- d-inline d-p"">Please note that requests relating to access by US public authorities for intelligence activities are subject to another procedure. Please contact your national DPA for more information.</p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-inline d-ref"" href=""#_ftnref1"" id=""ref_1"">
          <sup class=""d- d-inline d-sup"">
            <sup class=""d- d-inline d-sup"">[1]</sup>
          </sup>
        </a> In this context, European individuals means any natural person, regardless of his/her nationality, whose personal data have been transferred to a US company under the EU-US Privacy Shield.</p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-c-footnote d-inline d-ref"" refersto=""#_ftnref2"" id=""_ftn2"">[2]</a> The decision on the adequacy of the EU-U.S. Privacy Shield Framework (""Privacy Shield"") or (""Framework"") was adopted by the European Commission on July 12, 2016. It was designed by the European Commission and the U.S. Department of Commerce to replace the Safe-Harbor-<a class=""d- d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=32000D0520"" id=""ref_2"">Decision 2000/520/EC</a> which were declared invalid by the European Court of Justice in 6 October 2015.</p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-inline d-ref"" href=""#_ftnref3"" id=""ref_1"">
          <sup class=""d- d-inline d-sup"">
            <sup class=""d- d-inline d-sup"">[3]</sup>
          </sup>
        </a> For more detailed information as to the guarantees for the data transferred and as to your rights under the EU-U.S. Privacy Shield, please consult the <u class=""d- d-inline d-u""><a class=""d- d-inline d-ref"" target=""_blank"" href=""http://ec.europa.eu/justice/data-protection/files/eu-us_privacy_shield_guide_en.pdf"" id=""ref_1"">Guide to the EU-US Privacy Shield published by the European Commission.</a></u></p>
      <p class=""d- d-inline d-p"">
        <a class=""d- d-inline d-ref"" href=""#_ftnref4"" id=""ref_1"">
          <sup class=""d- d-inline d-sup"">
            <sup class=""d- d-inline d-sup"">[4]</sup>
          </sup>
        </a> Whenever the words ""national data protection authority"", ""EU DPA"" or ""EU handling authority"", this also refers to the EDPS, which will be the EU handling authority in case where your personal data have been transferred to an US Privacy Shield certified company by an EU institution.</p>
    </div>
  </div>", res.Items[0].Text);

            Assert.AreEqual(5780406, res.Meta.DocLangId);
            Assert.AreEqual("30003_GDPE", res.Meta.DocNumber);
            Assert.AreEqual("dd1841c7-2482-4c44-a512-ec9276870de1", res.Meta.Idenitifier);
            Assert.AreEqual("EU-US PRIVACY SHIELD F.A.Q. FOR EUROPEAN INDIVIDUALS", res.Meta.Title);
            Assert.AreEqual(DateTime.MinValue, res.Meta.PublicationDate);
            Assert.AreEqual(DateTime.MinValue, res.Meta.ActDate);
            Assert.AreEqual(new DateTime(2019, 6, 27), res.Meta.LastChangeDate);
            Assert.AreEqual("BG", res.Meta.Country);
            Assert.AreEqual("en", res.Meta.ShortLang);
            Assert.AreEqual("eng", res.Meta.Language);
            Assert.AreEqual(4, res.Meta.LangId);
        }

        [Test]
        public void TestGetSmeDocByDocNumber_LegislationDoc()
        {
            var res = this.docsService.GetSmeDocByDocNumber("32007D0533", 4, string.Empty);

            Assert.AreEqual(59, res.Items.Count);

            Assert.AreEqual(6210537, res.Meta.DocLangId);
            Assert.AreEqual("32007D0533", res.Meta.DocNumber);
            Assert.AreEqual("5b93ca5c-7d49-4b30-b70d-9d4204086a74", res.Meta.Idenitifier);
            Assert.AreEqual("Council Decision 2007/533/JHA of 12 June 2007 on the establishment, operation and use of the second generation Schengen Information System (SIS II)", res.Meta.Title);
            Assert.AreEqual(null, res.Meta.PublicationDate);
            Assert.AreEqual(new DateTime(2007, 6, 12), res.Meta.ActDate);
            Assert.AreEqual(new DateTime(2019, 7, 5), res.Meta.LastChangeDate);
            Assert.AreEqual("EU", res.Meta.Country);
            Assert.AreEqual("en", res.Meta.ShortLang);
            Assert.AreEqual("eng", res.Meta.Language);
            Assert.AreEqual(4, res.Meta.LangId);
        }

        [Test]
        public void TestGetSmeDocByDocNumber_CaseLawDoc()
        {
            var res = this.docsService.GetSmeDocByDocNumber("62013CA0615", 1, string.Empty);

            Assert.AreEqual(1, res.Items.Count);
            Assert.AreEqual(@"<div class=""d-EURLEX d-althierarchy d-judgmentBody"">
    <div class=""d-EURLEX d-althierarchy d-motivation"">
      <div class=""d-EURLEX d-blocksopt d-blockList"">
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p class=""d-EURLEX d-c-s-normal d-inline d-p"">Език на производството: английски</p>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p id=""s-d1e80-5-1"" class=""d-EURLEX d-c-s-ti-grseq-1 d-inline d-p"">
            <span class=""d-EURLEX d-c-s-bold d-inline d-span"">Страни</span>
          </p>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p class=""d-EURLEX d-c-s-normal d-inline d-p"">
            <span class=""d-EURLEX d-c-s-italic d-inline d-span"">Жалбоподатели:</span> ClientEarth, Pesticide Action Network Europe (PAN Europe) (представител: P. Kirch, avocat)</p>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p class=""d-EURLEX d-c-s-normal d-inline d-p"">
            <span class=""d-EURLEX d-c-s-italic d-inline d-span"">Други страни в производството:</span> Европейски орган за безопасност на храните (ЕОБХ) (представители: D. Detken, C. Pintado и R. Van der Hout, advocaat), Европейска комисия (представители: B. Martenczuk и L. Pignataro-Nolin)</p>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p class=""d-EURLEX d-c-s-normal d-inline d-p"">
            <span class=""d-EURLEX d-c-s-italic d-inline d-span"">Встъпила страна в подкрепа на другата страна в производтвото</span>: Европейски надзорен орган по защита на данните (ЕНОЗД) (представители: A. Buchta и M. Pérez Asinari)</p>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p id=""s-d1e99-5-1"" class=""d-EURLEX d-c-s-ti-grseq-1 d-inline d-p"">
            <span class=""d-EURLEX d-c-s-bold d-inline d-span"">Диспозитив</span>
          </p>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <table style=""border:0;width:100%;"" class=""d-EURLEX d-table"">
            <tr class=""d-EURLEX d-tr"">
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">1)</p>
              </td>
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">Отменя <a class=""d-EURLEX d-apis d-inline d-ref"" target=""_blank"" href=""./CELEX=62011TJ0214"">решението на Общия съд на Европейския съюз ClientEarth и PAN Europe/ЕОБХ (T-214/11, EU:T:2013:483</a>).</p>
              </td>
            </tr>
          </table>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <table style=""border:0;width:100%;"" class=""d-EURLEX d-table"">
            <tr class=""d-EURLEX d-tr"">
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">2)</p>
              </td>
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">Отменя решението на Европейския орган за безопасност на храните (ЕОБХ) от 12 декември 2011 г.</p>
              </td>
            </tr>
          </table>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <table style=""border:0;width:100%;"" class=""d-EURLEX d-table"">
            <tr class=""d-EURLEX d-tr"">
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">3)</p>
              </td>
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">Европейският орган за безопасност на храните (ЕОБХ) понася направените от него съдебни разноски и е осъден да заплати съдебните разноски на ClientEarth и Pesticide Action Network Europe (PAN Europe) в рамките на производството по обжалване и на първоинстанционното производство.</p>
              </td>
            </tr>
          </table>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <table style=""border:0;width:100%;"" class=""d-EURLEX d-table"">
            <tr class=""d-EURLEX d-tr"">
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">4)</p>
              </td>
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">Европейската комисия понася направените от нея съдебни разноски в производството по обжалване и в първоинстанционното производство.</p>
              </td>
            </tr>
          </table>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <table style=""border:0;width:100%;"" class=""d-EURLEX d-table"">
            <tr class=""d-EURLEX d-tr"">
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">5)</p>
              </td>
              <td rowspan=""1"" colspan=""1"" class=""d-EURLEX d-td"">
                <p class=""d-EURLEX d-c-s-normal d-inline d-p"">Европейският надзорен орган по защита на данните (ЕНОЗД) понася направените от нея съдебни разноски в производството по обжалване.</p>
              </td>
            </tr>
          </table>
        </div>
        <div class=""d-EURLEX d-blocksopt d-item"">
          <p class=""d-EURLEX d-c-s-note d-inline d-p"">
            <a class=""d-EURLEX d-c-footnote d-inline d-ref"" refersto=""#ntc1-C_2015311BG_01000501-E0001"" id=""ntr1-C_2015311BG_01000501-E0001"">(<span class=""d-EURLEX d-c-s-super d-inline d-span"">1</span>)</a>
            <a class=""d-EURLEX d-inline d-ref"" target=""_blank"" href=""http://eur-lex.europa.eu/legal-content/BG/AUTO/?uri=OJ:C:2014:071:TOC"" id=""ref_2"">ОВ C 71, 8.3.2014 г.</a>
          </p>
        </div>
      </div>
    </div>
  </div>", res.Items[0].Text);

            Assert.AreEqual(4777157, res.Meta.DocLangId);
            Assert.AreEqual("62013CA0615", res.Meta.DocNumber);
            Assert.AreEqual("84398662-5dab-4f04-ae46-9cfd8585714d", res.Meta.Idenitifier);
            Assert.AreEqual("Дело C-615/13 P : Решение на Съда (втори състав) от 16 юли 2015 г. — ClientEarth, Pesticide Action Network Europe (PAN Europe)/Европейски орган за безопасност на храните (ЕОБХ), Европейска комисия (Обжалване — Достъп до документите на институциите на Европейския съюз — Регламент (ЕО) № 1049/2001 — Член 4, параграф 1, буква б) — Регламент (ЕО) № 45/2001 — Член 8 — Изключение от правото на достъп — Защита на личните данни — Понятие за лични данни — Условия за предаване на лични данни — Име на автора на всяко становище по проект за насоки на Европейския орган за безопасност на храните (ЕОБХ) във връзка с научната литература, която трябва да се приложи към заявленията за издаване на разрешение за търговия на продукти за растителна защита — Отказ да се предостави достъп)", res.Meta.Title);
            Assert.AreEqual(null, res.Meta.PublicationDate);
            Assert.AreEqual(new DateTime(2015, 7, 16), res.Meta.ActDate);
            Assert.AreEqual(new DateTime(2019, 6, 26), res.Meta.LastChangeDate);
            Assert.AreEqual("EU", res.Meta.Country);
            Assert.AreEqual("bg", res.Meta.ShortLang);
            Assert.AreEqual("bul", res.Meta.Language);
            Assert.AreEqual(1, res.Meta.LangId);
        }

        [Test]
        public void TestGetSmeDocByDocIdentifier_DocConvert01()
        {
            var res = this.docsService.GetSmeDocByDocIdentifier("a32b9afc-c69c-4714-b4b1-67e76c0f54d5", string.Empty);

            Assert.AreEqual(4, res.Items.Count);

            Assert.AreEqual(res.Items?[0]?.Text, @"<p class=""d-SmeDataLegislation d-c-s-bold d-c-s-center d-inline d-p"">ЗАКОН за социалните услуги</p><p class=""d-SmeDataLegislation d-inline d-p"">
        <span class=""d-SmeDataLegislation d-inline d-span"">
          <span class=""d-SmeDataLegislation d-inline d-span"">Oбн</span>
        </span>
        <span class=""d-SmeDataLegislation d-inline d-span"">., ДВ, бр. 24 от 22.03.2019 г., в сила от 1.01.2020 г.</span>
      </p>");
        }

        [Test]
        public void TestGetSmeDocByDocIdentifier_DocConvert02()
        {
            var res = this.docsService.GetSmeDocByDocIdentifier("67a05be1-7a76-4cc5-bc15-cdc5452c9b62", string.Empty);

            Assert.AreEqual(49, res?.Items?.Count);
            Assert.AreEqual(res?.Items?[43]?.Heading, @"CHAPTER I");

            Assert.AreEqual(res?.Items?[43]?.Text, @"<a class=""doc-anchor"" eid=""chap_1""></a><div class=""d-EURLEX d-hierarchy d-chapter"">
<span class=""d-EURLEX d-inline d-num"">CHAPTER I</span>
<span class=""d-EURLEX d-inline d-heading""><span class=""d-EURLEX d-c-ti-section-2 d-inline d-span"">General provisions</span><br class=""d-EURLEX d-inline d-eol""></span></div>");
        }

        [Test]
        public void TestGetUpdatedDocumentsNoResult()
        {
            var input = new List<LastChangeOfDoc>() {
                new LastChangeOfDoc{ Ident= "b6696453-7a0b-4848-8c79-b21693ed79ea", LastChangeDate = DateTime.MinValue}
            };

            this.docsService.UpdateLastChangeDoc(input.First());
            input.First().LastChangeDate = new DateTime(2018, 01, 01);
            var res = this.docsService.GetUpdatedDocuments(input);
            Assert.AreEqual(0, res.Count);

        }

        [Test]
        public void TestGetUpdatedDocumentsOneResult()
        {
            var input = new List<LastChangeOfDoc>() {
                new LastChangeOfDoc{ Ident= "b6696453-7a0b-4848-8c79-b21693ed79ea", LastChangeDate = new DateTime(2018, 5, 5)}
            };
            this.docsService.UpdateLastChangeDoc(input.First());
            input.First().LastChangeDate = new DateTime(2018, 01, 01);
            var res = this.docsService.GetUpdatedDocuments(input);
            Assert.AreEqual(1, res.Count);

        }

        [Test]
        public void TestGetUpdatedDocumentsTwoResult()
        {
            var input = new List<LastChangeOfDoc>() {
                new LastChangeOfDoc{Ident = "a0ff7603-135f-46a3-9404-171b66a02615",LastChangeDate =new DateTime(2019, 01, 01)},
                new LastChangeOfDoc{Ident = "ffe52d01-2bc9-423f-98bf-a255bc0e384e",LastChangeDate = new DateTime(2019, 01, 01)}
            };
            input.ForEach(x => this.docsService.UpdateLastChangeDoc(x));
            input.ForEach(x => x.LastChangeDate = new DateTime(2018, 01, 01));
            var res = this.docsService.GetUpdatedDocuments(input);
            Assert.AreEqual(2, res.Count);

        }


    }
}