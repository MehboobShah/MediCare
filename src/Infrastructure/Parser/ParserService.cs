using Ardalis.Specification.EntityFrameworkCore;
using DocumentFormat.OpenXml.ExtendedProperties;
using DocumentFormat.OpenXml.Spreadsheet;
using Mapster;
using MediCare.Application.Auditing;
using MediCare.Application.Common.Persistence;
using MediCare.Application.Medical;
using MediCare.Application.Ontology;
using MediCare.Application.Processing;
using MediCare.Application.Report;
using MediCare.Application.Users;
using MediCare.Domain.Ontology;
using MediCare.Domain.Report;
using MediCare.Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Reflection;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MediCare.Infrastructure.Parser;
public class ParserService : IParserService
{
    private readonly IRepository<AnalyteResult> _analyteResultRepository;
    private readonly IRepository<PatientReport> _patientReportsRepository;
    private readonly IRepository<Patient> _patientRepository;
    private readonly ILabService _labService;
    private readonly IPatientReportService _patientReportService;
    private readonly IPatientService _patientService;
    private readonly ITestTypeService _testTypeService;
    private readonly IDictionaryService _dictionaryService;
    private readonly IAnalyteResultService _analyteResultService;
    private string reportText;
    private int i;
    private string word = string.Empty;
    string temp = string.Empty;
    private const string EscapeCharacters = "()[]{}-.:";
    private const string EscapeCharacters2 = "#-%";
    private List<Keyword> keywords;
    private Patient patient;

    private bool IfLabNameParsed { get; set; } = false;
    private bool LabTestNameParsed { get; set; } = false;
    private bool IsFieldNameParsed { get; set; }
    private bool IsValueParsed { get; set; }
    private bool IsFieldNameParsingStarted { get; set; }
    private bool IsValueParsingStarted { get; set; }
    private bool IsMultiField { get; set; }
    private bool IsAnalyteParsed { get; set; }
    private bool IsAnalyteStartRangeParsed { get; set; }
    private bool IsAnalyteEndRangeParsed { get; set; }
    private bool IsAnalyteResultParsed { get; set; }
    private List<string> Fields { get; set; }
    private List<string> Values { get; set; }
    private Stack<string> Brackets { get; set; }

    private readonly IStringLocalizer _t;

    public ParserService(
        IRepository<AnalyteResult> analyteResultRepository,
        IRepository<PatientReport> patientReportsRepository,
        IRepository<Patient> patientRepository,
        ILabService labService,
        ITestTypeService testTypeService,
        IPatientReportService patientReportService,
        IDictionaryService dictionaryService,
        IPatientService patientService,
        IAnalyteResultService analyteResultService,
        IStringLocalizer<ParserService> localizer)
    {
        _analyteResultRepository = analyteResultRepository;
        _patientReportsRepository = patientReportsRepository;
        _patientRepository = patientRepository;
        _labService = labService;
        _testTypeService = testTypeService;
        _patientReportService = patientReportService;
        _dictionaryService = dictionaryService;
        _patientService = patientService;
        _analyteResultService = analyteResultService;
        _t = localizer;
        Fields = new List<string>();
        Values = new List<string>();
        Brackets = new Stack<string>();

    }

    public async Task<bool> UploadPdfAsync(UploadPdfRequest request, string userId, CancellationToken cancellationToken)
    {
        AddPatientReportRequest patientReportRequest = new AddPatientReportRequest { LabName = request.LabName, TestType = request.TestType};
        var patientReportId = await _patientReportService.AddPatientReportAsync(patientReportRequest, userId, cancellationToken);

        reportText =
            @"Stadium Road, P.O. Box 3500, Karachi - 74800,
                                                        Pakistan
                                                        U.P. Morr Collection Unit Tel:(021) 36950168

Medical Record # : L25516744 (UM80387)
                  1

Patient Name            : SUFIYAN,ANIS                                                    Age / Gender : 21Y / Male
                                                                                               Location : UPMORE
Specimen ID             : 29082022:HR5112R
                                                                               Requesting Physician : Unknown
Clinical Information / Comments:                                                              Account # : C37130849 - OSR

None                                                                                     Requested on : 29/08/2022 - 11:06
                                                                                           Collected on : 29/08/2022 - 11:06
Test                                                    Result                             Reported on : 29/08/2022 - 17:44

[COMPLETE BLOOD COUNT]                                                                                     Normal Ranges
[HAEMOGLOBIN HAEMATOCRIT]
HAEMOGLOBIN                             ..............  13.4 g/dl                ..............  (12.3-16.6)
HAEMATOCRIT                             ..............  40.2 %                   ..............  (38.4-50.7)

R.B.C.                                  .............. 4.55 x10E12/L             ..............  (4.25-6.02)

M.C.V.                                  .............. 88.4 fL                   ..............  (78.7-96.3)

M.C.H.                                  .............. 29.5 pg                   ..............  (25.1-31.6)

M.C.H.C.                                .............. 33.3 g/dL                 ..............  (30-35.5)

R.D.W                                   .............. 13.1 %                    ..............  (12.1-16.9)
        This is an electronic report & not
W.B.C.                                  .............. 3.6 x10E9/L               ..............  (4.8-11.3)

                                        ..............  46.2 %                   ..............  (34.9-76.2)
                                        ..............  49.3 %                   ..............  (17.5-45)
to be used for any legal purposes NEUTROPHILS

LYMPHOCYTES

EOSINOPHILS                             .............. 0.3 %                     ..............  (0.3-7.4)

MONOCYTES                               .............. 3.6 %                     ..............  (3.9-10)

BASOPHILS                               .............. 0.6 %                     ..............  (0-1)

Neutrophils lymphocytes ratio (NLR)     .............. 0.9 ratio                 ..............  (1-4)

PLATELETS                               .............. 103 x10E9/L               ..............  (154-433)

        METHODOLOGY:
        The test is performed on Automated Haematology Analyzer.

PERIPHERAL FILM                         ..............

        NORMOCYTIC, NORMOCHROMIC

        PLATELETS LOW ON FILM

        LEUCOPENIA

        LEFT SHIFT NEUTROPHILS

        ? CAUSE

        Kindly Note reference Ranges change on 06/12/2018.

This is a computer generated report therefore does not require any signature.

Dr. Bushra Moiz         Dr. Mohammad Usman Shaikh       Dr. Salman Naseem Adil   Dr. Natasha Ali              Dr. Muhammad Shariq Shaikh
MBBS, FCPS(Hematology)  MBBS, FCPS(Hematology)          MBBS, FCPS (Hematology)  MBBS, FCPS (Hematology)      MBBS, FCPS (Hematology)
Associate Professor     Associate Professor             Professor                Associate Professor          Assistant Professor

Dr. Anila Rashid        Dr. Muhammad Hasan
MBBS, FCPS(Hematology)  MBBS, FCPS(Hematology)
Assistant Professor     Senior Instructor

        ".ToLower();

        var labs = await _labService.GetAllAsync(cancellationToken);
        var testTypes = await _testTypeService.GetAllTestTypeAsync(cancellationToken);
        var analytes = await _testTypeService.GetAllTestTypeAnalyteAsync("haematology", cancellationToken);
        keywords = await _dictionaryService.GetAllAsync(cancellationToken);
        patient = new Patient();
        var analyteResult = new AnalyteResult();

        for (i = 0; i < reportText.Length; i++)
        {

            if (IsAnalyteParsed && IsAnalyteResultParsed && IsAnalyteStartRangeParsed && IsAnalyteEndRangeParsed)
            {
                analyteResult.PatientReportId = patientReportId;
                await _analyteResultService.AddAnalyteResultAsync(analyteResult, cancellationToken);
                analyteResult = new AnalyteResult();
                IsAnalyteParsed = false;
                IsAnalyteResultParsed = false;
                IsAnalyteStartRangeParsed = false;
                IsAnalyteEndRangeParsed = false;
            }

            if (reportText[i] == ' ')
            {
                if (!string.IsNullOrEmpty(word))
                {

                    if (i + 1 < reportText.Length && !EscapeCharacters2.Contains(reportText[i + 1]) && (!char.IsLetterOrDigit(reportText[i + 1])))
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
                                word = string.Empty;
                                continue;

                            }

                        }

                        // done - last field of single field
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

                        // done - last field of multifield
                        if (IsFieldNameParsingStarted && !IsFieldNameParsed && IsMultiField)
                        {
                            IsFieldNameParsed = true;
                            IsValueParsingStarted = true;
                            Fields.Add(word);
                            word = string.Empty;
                            i++;
                            continue;

                        }

                        // done - last value of multi/single value
                        if (IsFieldNameParsed && IsValueParsingStarted)
                        {
                            if (EscapeCharacters.Contains(reportText[i + 1]))
                            {
                                word += reportText[i];
                                continue;
                            }
                            else
                            {
                                Values.Add(word);
                                word = string.Empty;
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
                            word = string.Empty;
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
                            word = string.Empty;
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
                if (!string.IsNullOrEmpty(word))
                {
                    Brackets.Push(word);
                    word = string.Empty;
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
            else if (char.IsLetterOrDigit(reportText[i]))
            {
                word += reportText[i];

            }
            else if ((IsFieldNameParsed || IsAnalyteParsed) && EscapeCharacters.Contains(reportText[i]))
            {
                if (!word.Any(char.IsLetterOrDigit))
                {
                    Fields.Clear();
                    IsValueParsingStarted = false;
                    IsValueParsed = false;
                    IsFieldNameParsingStarted = false;
                    IsFieldNameParsed = false;
                    IsMultiField = false;
                    word = string.Empty;
                }
                else
                {
                    word += reportText[i];
                }

                continue;

            }
            else if (i + 1 < reportText.Length && ((reportText[i] == '\r' && reportText[i + 1] == '\n') || (reportText[i] == '\n' && reportText[i + 1] == '\r')))
            {
                if (!string.IsNullOrEmpty(word))
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
                if (!string.IsNullOrEmpty(word))
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
                if (char.IsDigit(reportText[i + 1]))
                {
                    word += reportText[i];
                    continue;
                }

                FieldValueParser();
                i--;
                continue;
            }

        }

        bool result = await _patientService.AddPatientAsync(patient, cancellationToken);
        var currentDir = System.IO.Directory.GetCurrentDirectory();
        System.IO.File.WriteAllText(currentDir + "\\Results\\patient_" + patient.Id.ToString() + ".txt", JsonConvert.SerializeObject(patient));

        return result;
    }

    private void FieldValueParser()
    {
        if (IsAnalyteParsed)
        {
            word += reportText[i];
            i++;
        }

        // done - first field of multifield
        else if (!IsFieldNameParsingStarted)
        {
            IsMultiField = true;
            IsFieldNameParsingStarted = true;
            Fields.Add(word);
            word = string.Empty;
            i++;
        }

        // done - middle field of multifield
        else if (IsFieldNameParsingStarted && !IsFieldNameParsed && IsMultiField)
        {
            Fields.Add(word);
            if (reportText[i + 1] != '/')
            {
                IsFieldNameParsed = true;

            }
            word = string.Empty;
            i++;
        }

        // done - middle value of multivalue
        else if (IsFieldNameParsed && IsValueParsingStarted && IsMultiField)
        {
            Values.Add(word);
            word = string.Empty;
            i++;
        }
        else if (IsFieldNameParsed && !IsValueParsed && !IsMultiField)
        {
            if (Fields[0] == "name")
            {
                word += reportText[i];
                word += reportText[++i];

            }
        }

        // done - first value of multivalue
        else if (IsFieldNameParsed && !IsValueParsingStarted && IsMultiField)
        {
            IsValueParsingStarted = true;
            Values.Add(word);
            word = string.Empty;
            i++;
        }
    }

    private bool LabNameParser(List<LabDto> labs)
    {
        var exists = labs.Where(x => x.Name.Contains(word)).FirstOrDefault();
        if (exists != null)
        {
            IfLabNameParsed = true;
            word = string.Empty;
            return true;
        }

        return false;
    }

    private bool LabTestNameParser(List<TestTypeDto> testTypes)
    {
        var exists = testTypes.Where(x => x.Name.Contains(word)).FirstOrDefault();
        if (exists != null)
        {
            LabTestNameParsed = true;
            word = string.Empty;
            return true;
        }

        return false;
    }

    private string ExtractKeyword(string word)
    {
        var result = keywords.Where(kw =>

            kw.Dictionaries.Any(dictionary => word.Trim().Contains(dictionary.Name))).ToList();

        if (result != null && result.Count > 0)
        {
            if (result.Find(k => k.Name == "ignore") != null)
            {
                return "ignore";
            }
            else
            {
                return result.FirstOrDefault().Name;
            }
        }
        else
        {
            return "Invalid Keyword";
        }

    }

    private string CheckMaxFieldLength(string temp, string keyword)
    {

        if (keyword == "Invalid Keyword")
        {
            temp += word + " ";
            word = string.Empty;
        }
        else
        {
            Fields.Add(temp + keyword);
            temp = string.Empty;
            word = string.Empty;

        }

        return temp;
    }

    private void ParseFieldAndValueLists()
    {
        if (Fields.Count == Values.Count)
        {
            for (int j = 0; j < Fields.Count; j++)
            {
                HandleKeyword(ExtractKeyword(Fields[j]), Values[j]);

            }

            Fields.Clear();
            Values.Clear();
            IsMultiField = false;
            IsFieldNameParsed = false;
            IsValueParsed = false;
            IsFieldNameParsingStarted = false;
            IsValueParsingStarted = false;
            word = string.Empty;
        }

    }

    private void HandleKeyword(string keyword, string value = "")
    {

        switch (keyword)
        {
            case "ignore":
                word = string.Empty;
                break;
            case "receipt":
                patient.Receipt = value;
                break;
            case "name":
                patient.Name = value;
                break;
            case "age":
                patient.Age = value;
                break;
            case "gender":
                patient.Gender = value;
                break;
            case "location":
                patient.Location = value;
                break;
            case "medical record no":
                patient.MedicalRecordNo = value;
                break;
            case "specimen no":
                patient.SpecimenNo = value;
                break;
            case "requested on":
                patient.RequestedOn = value;
                break;
            case "reported on":
                patient.ReportedOn = value;
                break;
            case "collected on":
                patient.CollectedOn = value;
                break;
            case "account no":
                patient.AccountNo = value;
                break;
            case "referred by":
                patient.ReferredBy = value;
                break;
            case "report no":
                patient.MedicalReportNo = value;
                break;
            case "bed":
                patient.BedNo = value;
                break;
            case "ward":
                patient.Ward = value;
                break;
            case "Invalid Keyword":

                break;
        }
    }
}
