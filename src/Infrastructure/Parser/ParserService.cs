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
using MediCare.Application.Result;
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
using static Microsoft.Extensions.Logging.EventSource.LoggingEventSource;

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
    string analyteTemp = string.Empty;
    private const string EscapeCharacters = "()[]{}-.";
    private const string EscapeCharacters2 = "#-%";
    private List<Keyword> keywords;
    private List<TestTypeAnalyteDto> analytes;
    private PatientReport patientReport;
    private AnalyteResult analyteResult;

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
    private Queue<string>? TempFieldQueue { get; set; }
    private bool isPreviousFieldExist { get; set; }
    private bool isPreviousFieldExistAfterNewLine { get; set; }

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
        analyteResult = new AnalyteResult();

    }

    public async Task<PatientDetailsDto> UploadPdfAsync(UploadPdfRequest request, string userId, CancellationToken cancellationToken)
    {
        AddPatientReportRequest patientReportRequest = new AddPatientReportRequest { LabName = request.LabName, TestType = request.TestType};
        patientReport = await _patientReportService.AddPatientReportAsync(patientReportRequest, userId, cancellationToken);

        reportText = request.ReportText.ToLower();
        var labs = await _labService.GetAllAsync(cancellationToken);
        var testTypes = await _testTypeService.GetAllTestTypeAsync(cancellationToken);
        analytes = await _testTypeService.GetAllTestTypeAnalyteAsync(request.TestType, cancellationToken);
        keywords = await _dictionaryService.GetAllAsync(cancellationToken);
        //patientReport = new PatientReport();
        var analyteResultList = new List<AnalyteResultDto>();

        for (i = 0; i < reportText.Length; i++)
        {

            if (IsAnalyteParsed && IsAnalyteResultParsed && IsAnalyteStartRangeParsed && IsAnalyteEndRangeParsed)
            {
                analyteResult.PatientReportId = patientReport.Id;
                await _analyteResultService.AddAnalyteResultAsync(analyteResult, cancellationToken);
                analyteResultList.Add(analyteResult.Adapt<AnalyteResultDto>());
                analyteResult = new AnalyteResult();
                IsAnalyteParsed = false;
                IsAnalyteResultParsed = false;
                IsAnalyteStartRangeParsed = false;
                IsAnalyteEndRangeParsed = false;
            }

            if (i == 2300)
            {

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

                            if (ExtractAnalyte(analyteTemp + word))
                            {
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
                                else if (isPreviousFieldExist)
                                {


                                    Fields.Add(TempFieldQueue.Dequeue());

                                    if (TempFieldQueue.Count == 0)
                                    {
                                        TempFieldQueue = null;
                                        isPreviousFieldExist = false;
                                        isPreviousFieldExistAfterNewLine = false;
                                    }

                                    Values.Add(word);
                                    ParseFieldAndValueLists();
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
                            var keyword = ExtractKeyword(word);
                            if (keyword != "Invalid Keyword" && keyword != "ignore")
                            {
                                IsFieldNameParsed = true;
                                IsValueParsingStarted = true;
                                Fields.Add(keyword);

                            }
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

                                if (CheckIfFieldisKeywordOrNot())
                                {
                                    continue;
                                }

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
                            var keyword = ExtractKeyword(word);
                            if (keyword != "Invalid Keyword" && keyword != "ignore")
                            {
                                IsFieldNameParsed = true;
                                Fields.Add(keyword);

                            }

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
                            if (!FieldValueParser())
                            {
                                continue;
                            }

                            i--;
                        }
                        else
                        {
                            if (keyword != "Invalid Keyword" && keyword != "ignore")
                            {
                                Fields.Add(keyword);
                                IsFieldNameParsingStarted = true;
                                IsFieldNameParsed = true;

                            }
                            word = string.Empty;
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
                    if (keyword == "Invalid Keyword")
                    {
                        if (!IsAnalyteParsed && ExtractAnalyte(analyteTemp + "(" + word + ")"))
                        {
                            i++;
                            continue;
                        }
                    }
                    else
                    {
                        HandleKeyword(keyword);
                    }

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
                if (isPreviousFieldExist)
                {
                    isPreviousFieldExistAfterNewLine = true;
                }

                if (!string.IsNullOrEmpty(word))
                {
                    string keyword = ExtractKeyword(temp + word);
                    if (IsFieldNameParsed)
                    {
                        if (CheckIfFieldisKeywordOrNot())
                        {
                            i = i + 1;
                            continue;
                        }

                        Values.Add(word);
                        ParseFieldAndValueLists();
                    }
                    else if (LabTestNameParser(testTypes) || LabNameParser(labs))
                    {
                        i = i + 1;
                        continue;
                    }
                    else if (keyword == "Invalid Keyword" && IsAnalyteParsed)
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
 
                    }
                    else if((keyword == "Invalid Keyword" || keyword == "ignore") && !IsAnalyteParsed)
                    {
                        word = string.Empty;
                        temp = string.Empty;

                    }

                }

                i = i + 1;
                continue;
            }
            else if (reportText[i] == '\r' || reportText[i] == '\n')
            {
                if (!string.IsNullOrEmpty(word))
                {
                    HandleKeyword(ExtractKeyword(word));
                }
                else
                {
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

                if(FieldValueParser()) i--;
                continue;
            }
            else
            {
                continue;
            }

        }

        var result = await _patientService.AddPatientAsync(patientReport, cancellationToken);
        var currentDir = System.IO.Directory.GetCurrentDirectory();
        //System.IO.File.WriteAllText(currentDir + "\\Results\\patient_" + patientReport.Id.ToString() + ".txt", JsonConvert.SerializeObject(patientReport));

        return new PatientDetailsDto
        {
            PatientDetails = result,
            AnalyteResultDetails = analyteResultList
        };
    }

    private bool FieldValueParser()
    {
        if (IsAnalyteParsed)
        {
            word += reportText[i];
            i++;
        }

        // done - first field of multifield
        else if (!IsFieldNameParsingStarted)
        {
            var keyword = ExtractKeyword(word);
            if (keyword != "Invalid Keyword" && keyword != "ignore")
            {
                IsMultiField = true;
                IsFieldNameParsingStarted = true;
                Fields.Add(keyword);

            }
            word = string.Empty;
            i++;
        }

        // done - middle field of multifield
        else if (IsFieldNameParsingStarted && !IsFieldNameParsed && IsMultiField)
        {
            var keyword = ExtractKeyword(word);
            if (keyword != "Invalid Keyword" && keyword != "ignore")
            {
                Fields.Add(keyword);

            }

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
            if (CheckIfFieldisKeywordOrNot())
            {
                return false;
            }

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
            if (CheckIfFieldisKeywordOrNot())
            {
                return false;
            }

            IsValueParsingStarted = true;
            Values.Add(word);
            word = string.Empty;
            i++;
        }

        return true;
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

    private bool CheckIfFieldisKeywordOrNot()
    {
        //if (!isPreviousFieldExistAfterNewLine)
        //{
        //    return false;
        //}

        string keyword = ExtractKeyword(temp + word);
        if (keyword != "Invalid Keyword")
        {
            if (Fields.Count > 0)
            {
                if (TempFieldQueue == null)
                {
                    TempFieldQueue = new Queue<string>(Fields);
                }
                else
                {
                    Fields.Distinct().ToList().ForEach(TempFieldQueue.Enqueue);
                }

                isPreviousFieldExist = true;

                Fields.Clear();
            }

            temp = CheckMaxFieldLength(temp, keyword);
            IsFieldNameParsingStarted = true;
            IsFieldNameParsed = true;
            IsValueParsingStarted = true;
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

    private bool ExtractAnalyte(string analytestring)
    {
        var analyte = analytes.Where(p => p.Name.Trim().Contains(analytestring.Trim())).FirstOrDefault();

        if (analyte != null)
        {
            if (analyte.Name.Trim() == analytestring.Trim() || (i + 1 < reportText.Length && reportText[i + 1] == ' '))
            {
                analyteResult.AnalyteId = analyte.Id;
                IsAnalyteParsed = true;
                word = string.Empty;
                analyteTemp = string.Empty;
            }
            else
            {
                analyteTemp += word + " ";
                word = string.Empty;

            }

            return true;
        }

        return false;
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
                patientReport.Receipt = value;
                break;
            case "name":
                patientReport.Name = value;
                break;
            case "age":
                patientReport.Age = value;
                break;
            case "gender":
                patientReport.Gender = value;
                break;
            case "location":
                patientReport.Location = value;
                break;
            case "medical record no":
                patientReport.MedicalRecordNo = value;
                break;
            case "specimen no":
            case "patient id":
                patientReport.SpecimenNo = value;
                break;
            case "requested on":
                patientReport.RequestedOn = value;
                break;
            case "reported on":
                patientReport.ReportedOn = value;
                break;
            case "collected on":
                patientReport.CollectedOn = value;
                break;
            case "account no":
                patientReport.AccountNo = value;
                break;
            case "referred by":
                patientReport.ReferredBy = value;
                break;
            case "report no":
                patientReport.MedicalReportNo = value;
                break;
            case "bed":
                patientReport.BedNo = value;
                break;
            case "ward":
                patientReport.Ward = value;
                break;
            case "Invalid Keyword":

                break;
        }
    }
}
