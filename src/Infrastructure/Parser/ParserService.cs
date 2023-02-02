using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Processing;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace BBD.WebApi.Infrastructure.Exercises;
public class ParserService : IParserService
{
    private readonly IRepository<AnalyteResult> _analyteResultRepository;
    private readonly IRepository<PatientReport> _patientReportsRepository;
    private readonly IRepository<Patient> _patientRepository;

    private readonly IStringLocalizer _t;

    public ParserService(
        IRepository<AnalyteResult> analyteResultRepository,
        IRepository<PatientReport> patientReportsRepository,
        IRepository<Patient> patientRepository,
        IStringLocalizer<ParserService> localizer)
    {
        _analyteResultRepository = analyteResultRepository;
        _patientReportsRepository = patientReportsRepository;
        _patientRepository = patientRepository;
        _t = localizer;

    }

    public async Task<bool> UploadPdfAsync(UploadPdfRequest request, CancellationToken cancellationToken)
    {

        reportText = @"                                     SAIFEE HOSPITAL          (Managed By Saifee Hospital Trust)


                                                    LABORATORY REPORT                                                   
        Receipt / MR #          SNR-202206-00047 / PAK SUZUKI [31152]                                                   Report #        DRP-202206-00614
        Patient                 01012725 / ZAINAB NAUSHAD BAIG
        Referred by             DR.SAIMA GHAYAS                                                                         Age / Gender 25 Year / Female
        Ward/Bed #              -                                                                                       Requested on 02/06/2022 - 10:13 PM
        Specimen #              DSC-202206-00283             Collected on      03/06/2022 - 02:11 AM (C1)               Reported on 03/06/2022 - 03:12 AM

                                                          HAEMATOLOGY
                                                   Historical Results           Current
        Test                                                                           Result                           Reference Range(Female)

        HAEMOGLOBIN                                                                     11 g/dL             ..........   11.5 - 15.4
        ERYTHROCYTES COUNT                                                              3.89 10E12/L        ..........   3.8 - 5.2
        HAEMATOCRIT                                                                     31.9 %              ..........   35 - 47
        MCV                                                                             82 fL               ..........   80 - 100
        MCH                                                                             28.3 Pg             ..........   27 - 34
        MCHC                                                                            34.5 g/dL           ..........   31 - 36

        TOTAL LEUCOCYTE COUNT                                                           9 x10E9/L           ..........   4 - 10

        NEUTROPHILS                                                                     67.2 %              ..........   40 - 80
        LYMPHOCYTES                                                                     24.1 %              ..........   20 - 40
        MONOCYTES                                                                       7.6 %               ..........   2 - 10
        EOSINOPHILS                                                                     1.1 %               ..........   0.3 - 7.4

        PLATELETS COUNT                                                                 235 x10E9/L         ..........   150 - 450


        MORPHOLOGY OF RBCs:                           Normocytic, Normochromic.

         


        Software Developed By ISR Software Services(Pvt.) Ltd.                                                                    Reported by : UMER FAREED (LAB)
        Note : This is a computer generated report, therefore signatures are not required.        Printed on / by : 03/06/2022 03:20 AM / UMER FAREED (LAB)
        DR.NAZISH KASHIF
        M.C.P.S
        HAEMATOLOGIST


                 ST-1 Block-F North Nazimabad, Karachi-74700. Tel: 36789400,36690696,36649866
                 Cell: 0346-8229956-60 Fax: 36724900,36628206 Email: info@saifeehospital.com.pk
                                       Website: www.saifeehospital.com.pk
        -----------------------Page 1 End-----------------------

        ".ToLower();

        List<Lab> labs = LabServices.GetLabs();
        List<TestType> testTypes = TestTypeServices.GetTestTypes();
        List<AnalyteDto> analytes = TestTypeServices.GetTestTypeAnalytes("haematology");

        for (i = 0; i < reportText.Length; i++)
        {

            if (IsAnalyteParsed && IsAnalyteResultParsed && IsAnalyteStartRangeParsed && IsAnalyteEndRangeParsed)
            {
                analyteResult.PatientreportId = patientReportId;
                AnalyteResultServices.InsertAnalyteResult(analyteResult);
                analyteResult = new AnalyteResult();
                IsAnalyteParsed = false;
                IsAnalyteResultParsed = false;
                IsAnalyteStartRangeParsed = false;
                IsAnalyteEndRangeParsed = false;
            }

            if (reportText[i] == ' ')
            {
                if (!String.IsNullOrEmpty(word))
                {

                    if (i + 1 < reportText.Length && !escapeCharacters2.Contains(reportText[i + 1]) && (!Char.IsLetterOrDigit(reportText[i + 1])))
                    {

                        if (reportText[i + 1] == '/')
                        {
                            FieldValueParser();
                            continue;
                        }
                        if (!IsAnalyteParsed)
                        {
                            var analyte = analytes.Where(p => p.Name.Trim() == word.Trim()).FirstOrDefault();

                            if (analyte != null)
                            {
                                analyteResult.AnalyteId = analyte.Id;
                                IsAnalyteParsed = true;
                                word = String.Empty;
                                continue;

                            }

                        }
                        //done - last field of single field
                        if (!IsFieldNameParsingStarted)
                        {
                            if (!IfLabNameParsed && LabNameParser(labs))
                            {

                                continue;
                                //var exists = labs.Where(x => x.Name.Contains(word)).FirstOrDefault();
                                //if (exists != null)
                                //{
                                //    IfLabNameParsed = true;
                                //    word = String.Empty;
                                //    continue;
                                //}
                            }

                            if (!LabTestNameParsed && LabTestNameParser(testTypes))
                            {

                                continue;
                                //var exists = testTypes.Where(x => x.Name.Contains(word)).FirstOrDefault();
                                //if (exists != null)
                                //{
                                //    LabTestNameParsed = true;
                                //    word = String.Empty;
                                //    continue;
                                //}

                            }

                            var keyword = ExtractKeyword(temp + word);
                            if (keyword == "Invalid Keyword")
                            {
                                if (IsAnalyteParsed)
                                {
                                    if (!IsAnalyteResultParsed && !word.Contains('-'))
                                    {
                                        analyteResult.Result = word;
                                        IsAnalyteResultParsed = true;
                                    }
                                    else if (word.Contains('-') && !IsAnalyteStartRangeParsed && !IsAnalyteEndRangeParsed)
                                    {
                                        string[] ranges = word.Split('-');
                                        analyteResult.StartRange = ranges[0];
                                        analyteResult.EndRange = ranges[1];
                                        IsAnalyteStartRangeParsed = true;
                                        IsAnalyteEndRangeParsed = true;
                                    }

                                }
                                word = string.Empty;
                                temp = string.Empty;
                                continue;
                            }

                            if (keyword == "ignore")
                            {
                                word = string.Empty;
                                temp = string.Empty;
                                continue;
                            }

                            temp = CheckMaxFieldLength(temp, keyword);

                            IsFieldNameParsingStarted = true;
                            IsFieldNameParsed = true;
                            IsValueParsingStarted = true;
                            i++;
                            continue;
                        }
                        //done - last field of multifield
                        if (IsFieldNameParsingStarted && !IsFieldNameParsed && IsMultiField)
                        {
                            IsFieldNameParsed = true;
                            IsValueParsingStarted = true;
                            Fields.Add(word);
                            word = String.Empty;
                            i++;
                            continue;

                        }
                        //done - last value of multi/single value
                        if (IsFieldNameParsed && IsValueParsingStarted)
                        {
                            if (escapeCharacters.Contains(reportText[i + 1]))
                            {
                                word += reportText[i];
                                continue;
                            }
                            else
                            {
                                Values.Add(word);
                                word = String.Empty;
                                IsFieldNameParsed = false;
                                IsFieldNameParsingStarted = false;
                                IsValueParsed = false;
                                IsValueParsingStarted = false;
                                IsMultiField = false;
                                ParseFieldAndValueLists();
                                i++;
                                continue;
                            }
                        }
                        if (!IsFieldNameParsed && !IsValueParsed)
                        {
                            IsFieldNameParsed = true;
                            Fields.Add(word);
                            word = String.Empty;
                            continue;
                        }
                        if (IsFieldNameParsed && IsValueParsed && IsMultiField)
                        {
                            ParseFieldAndValueLists();
                            continue;
                        }

                    }
                    else
                    {
                        string keyword = ExtractKeyword(word);

                        if (keyword == "Invalid Keyword")
                        {
                            word += reportText[i];
                            word += reportText[++i];
                        }
                        else if (IsMultiField)
                        {
                            FieldValueParser();
                            i--;
                        }
                        else
                        {
                            Fields.Add(word);
                            word = String.Empty;
                            IsFieldNameParsingStarted = true;
                            IsFieldNameParsed = true;
                        }


                    }

                }
                else
                {
                    continue;
                }

            }
            else if (reportText[i] == '(')
            {
                if (!String.IsNullOrEmpty(word))
                {
                    Brackets.Push(word);
                    word = "";
                    while (i + 1 < reportText.Length && reportText[i + 1] != ')')
                    {
                        i++;
                        word += reportText[i];
                    }
                    var keyword = ExtractKeyword(word);
                    if (keyword == "ignore")
                    {
                        HandleKeyword(keyword);
                        word = Brackets.Pop();
                        i++;
                        continue;
                    }
                    else
                    {
                        word = Brackets.Pop() + "(" + word + ")";
                        i++;
                        continue;
                    }

                }
                else
                {
                    while (i + 1 < reportText.Length && reportText[i + 1] != ')')
                    {
                        i++;
                        word += reportText[i];
                    }
                    var keyword = ExtractKeyword(word);
                    HandleKeyword(keyword);
                    i++;
                    continue;
                }

            }
            else if (Char.IsLetterOrDigit(reportText[i]))
            {
                word += reportText[i];


            }
            else if ((IsFieldNameParsed || IsAnalyteParsed) && escapeCharacters.Contains(reportText[i]))
            {
                if (!word.Any(Char.IsLetterOrDigit))
                {
                    Fields.Clear();
                    IsValueParsingStarted = false;
                    IsValueParsed = false;
                    IsFieldNameParsingStarted = false;
                    IsFieldNameParsed = false;
                    IsMultiField = false;
                    word = String.Empty;
                }
                else
                {
                    word += reportText[i];
                }

                continue;

            }
            else if (i + 1 < reportText.Length && (reportText[i] == '\r' && reportText[i + 1] == '\n') || (reportText[i] == '\n' && reportText[i + 1] == '\r'))
            {
                if (!String.IsNullOrEmpty(word))
                {
                    if (IsFieldNameParsed)
                    {
                        Values.Add(word);
                        ParseFieldAndValueLists();
                    }
                    else if (LabTestNameParser(testTypes) || LabNameParser(labs))
                    {
                        continue;
                    }
                    else if (ExtractKeyword(temp + word) == "Invalid Keyword" && IsAnalyteParsed)
                    {
                        if (!IsAnalyteResultParsed && !word.Contains('-'))
                        {
                            analyteResult.Result = word;
                            IsAnalyteResultParsed = true;
                        }
                        else if (word.Contains('-') && !IsAnalyteStartRangeParsed && !IsAnalyteEndRangeParsed)
                        {
                            string[] ranges = word.Split('-');
                            analyteResult.StartRange = ranges[0].Trim();
                            analyteResult.EndRange = ranges[1].Trim();
                            IsAnalyteStartRangeParsed = true;
                            IsAnalyteEndRangeParsed = true;
                        }

                        word = string.Empty;
                        temp = string.Empty;
                        continue;
                    }


                }
                else
                {
                    i = i + 2;
                    continue;
                }
            }
            else if (reportText[i] == '\r' || reportText[i] == '\n')
            {
                if (!String.IsNullOrEmpty(word))
                {
                    HandleKeyword(ExtractKeyword(word));
                }
                else
                {
                    i = i + 1;
                    continue;
                }
            }
            else if (reportText[i] == '/')
            {
                if (Char.IsDigit(reportText[i + 1]))
                {
                    word += reportText[i];
                    continue;
                }
                FieldValueParser();
                i--;
                continue;
            }

        }

        PatientServices.InsertPatient(patient);

        string result = "";

        return result;
    }

}
