/* eslint-disable no-undef */
const https = require("https");
const xml2js = require("xml2js");
const lineReader = require("line-reader");
const fs = require("fs");
const REPO_FILE = "./repo.txt";
const UNIT_TEST_RESULT_PATH = "../HungryNowTest/TestResults/results.xml";
const SCORE_PATH = "./scores.json";
const scores = require(SCORE_PATH);

const getCommitId = filePath => {
  return new Promise((resolve, reject) => {
    lineReader.eachLine(filePath, line => {
      resolve(line);
    });
  });
};

const getXMLData = () => {
  return fs.readFileSync(UNIT_TEST_RESULT_PATH, "utf8");
};

const testResultsToJson = xmlData => {
  const parser = new xml2js.Parser();
  return parser.parseStringPromise(xmlData);
};

const getJsonTestResults = async() => {
  const xmlData = getXMLData();
  return testResultsToJson(xmlData);
};

const postData = async () => {
  const repoName = process.env.CODE_COMMIT_REPO;
  const jsonData = await getJsonTestResults();
  const result = jsonData.TestRun.Results;
  const numTotalTests = result[0].UnitTestResult.length;
  const startTime = jsonData.TestRun.Times[0].$.start;
  const {bugs, features} = scores;

  const summary = {
    date: new Date(startTime),
    numTotalTests,
  };

  const bugFixing = [];
  const featureImplementation = [];
  let successCount = 0;

  bugs.forEach(bug => {
    result.forEach(unitTests => {
      unitTests.UnitTestResult.forEach(test => {
        if (test.$.testName.toLowerCase().replace(/\s/g, '') === bug.name.toLowerCase().replace(/\s/g, '')) {
          if (test.$.outcome === 'Failed') {
            isSucceed = false;
          } else {
            isSucceed = true;
            successCount++;
          }
          testResult = {
            fullName: bug.name,
            success: isSucceed,
            score: bug.score
          };
          bugFixing.push(testResult);
        }
      })
    })
  });

  features.forEach(feature => {
    result.forEach(unitTests => {
      unitTests.UnitTestResult.forEach(test => {
        if (test.$.testName.toLowerCase().replace(/\s/g, '') === feature.name.toLowerCase().replace(/\s/g, '')) {
          if (test.$.outcome === 'Failed') {
            isSucceed = false;
          } else {
            isSucceed = true;
            successCount++;
          }
          testResult = {
            fullName: feature.name,
            success: isSucceed,
            score: feature.score
          };
          featureImplementation.push(testResult);
        }
      })
    })
  });

  const params = {
    repoName,
    summary,
    bugFixing,
    featureImplementation
  }
  return params;
};

const sendReportData = async () => {
  const data = await postData();
  console.log(data);
  const options = {
    hostname: "app.devgrade.io",
    path: "/assessments/report",
    method: "POST",
    headers: {
      "Content-Type": "application/json",
      "Content-Length": Buffer.byteLength(JSON.stringify(data))
    }
  };

  const req = https.request(options, res => { });

  req.write(JSON.stringify(data));
  req.end();
};

sendReportData();