//Loading in a data-set and creating a data pipeline

using Microsoft.ML.Data;

namespace CreditCardFraudDetection.DataModels
{
    public class ModelInput
    {
        [ColumnName("Time"), LoadColumn(0)]
        public float Time { get; set; }

        [ColumnName("V1"), LoadColumn(1)]
        public float V1 { get; set; }

        [ColumnName("V2"), LoadColumn(2)]
        public float V2 { get; set; }

        [ColumnName("V3"), LoadColumn(3)]
        public float V3 { get; set; }

        [ColumnName("V4"), LoadColumn(4)]
        public float V4 { get; set; }

        [ColumnName("V5"), LoadColumn(5)]
        public float V5 { get; set; }

        [ColumnName("V6"), LoadColumn(6)]
        public float V6 { get; set; }

        [ColumnName("V7"), LoadColumn(7)]
        public float V7 { get; set; }

        [ColumnName("V8"), LoadColumn(8)]
        public float V8 { get; set; }

        [ColumnName("V9"), LoadColumn(9)]
        public float V9 { get; set; }

        [ColumnName("V10"), LoadColumn(10)]
        public float V10 { get; set; }

        [ColumnName("V11"), LoadColumn(11)]
        public float V11 { get; set; }

        [ColumnName("V12"), LoadColumn(12)]
        public float V12 { get; set; }

        [ColumnName("V13"), LoadColumn(13)]
        public float V13 { get; set; }

        [ColumnName("V14"), LoadColumn(14)]
        public float V14 { get; set; }

        [ColumnName("V15"), LoadColumn(15)]
        public float V15 { get; set; }

        [ColumnName("V16"), LoadColumn(16)]
        public float V16 { get; set; }

        [ColumnName("V17"), LoadColumn(17)]
        public float V17 { get; set; }

        [ColumnName("V18"), LoadColumn(18)]
        public float V18 { get; set; }

        [ColumnName("V19"), LoadColumn(19)]
        public float V19 { get; set; }

        [ColumnName("V20"), LoadColumn(20)]
        public float V20 { get; set; }

        [ColumnName("V21"), LoadColumn(21)]
        public float V21 { get; set; }

        [ColumnName("V22"), LoadColumn(22)]
        public float V22 { get; set; }

        [ColumnName("V23"), LoadColumn(23)]
        public float V23 { get; set; }

        [ColumnName("V24"), LoadColumn(24)]
        public float V24 { get; set; }

        [ColumnName("V25"), LoadColumn(25)]
        public float V25 { get; set; }

        [ColumnName("V26"), LoadColumn(26)]
        public float V26 { get; set; }

        [ColumnName("V27"), LoadColumn(27)]
        public float V27 { get; set; }

        [ColumnName("V28"), LoadColumn(28)]
        public float V28 { get; set; }

        [ColumnName("Amount"), LoadColumn(29)]
        public float Amount { get; set; }

        [ColumnName("Class"), LoadColumn(30)]
        public bool Class { get; set; }
    }
}


//Output model

using Microsoft.ML.Data;

namespace CreditCardFraudDetection.DataModels
{
    public class ModelOutput
    {
        [ColumnName("PredictedLabel")]
        public bool Prediction { get; set; }

        public float Score { get; set; }
    }
}

//load actual data into memory

IDataView trainingDataView = mlContext.Data.LoadFromTextFile<ModelInput>(
                                            path: dataFilePath,
                                            hasHeader: true,
                                            separatorChar: ',',
                                            allowQuoting: true,
                                            allowSparse: false);

//Creating and training a model

var dataProcessPipeline = mlContext.Transforms.Concatenate("Features", new[] { "Time", "V1", "V2", "V3", "V4", "V5", "V6", "V7", "V8", "V9", "V10", "V11", "V12", "V13", "V14", "V15", "V16", "V17", "V18", "V19", "V20", "V21", "V22", "V23", "V24", "V25", "V26", "V27", "V28", "Amount" });


// Choosing algorithm
var trainer = mlContext.BinaryClassification.Trainers.LightGbm(labelColumnName: "Class", featureColumnName: "Features");
// Appending algorithm to pipeline
var trainingPipeline = dataProcessPipeline.Append(trainer);

//train the model with the Fit method and save it using mlContext.Model.Save

ITransformer model = trainingPipeline.Fit(trainingDataView);
mlContext.Model.Save(model , trainingDataView.Schema, <path>);

var crossValidationResults = mlContext.BinaryClassification.CrossValidateNonCalibrated(trainingDataView, trainingPipeline, numberOfFolds: 5, labelColumnName: "Class");

//prediction

var predEngine = mlContext.Model.CreatePredictionEngine<ModelInput, ModelOutput>(mlModel);

ModelInput sampleData = new ModelInput() {
    time = 0,
    V1 = -1.3598071336738,
    ...
};

ModelOutput predictionResult = predEngine.Predict(sampleData);

Console.WriteLine($"Actual value: {sampleData.Class} | Predicted value: {predictionResult.Prediction}");

//tensorflow model

private ITransformer SetupMlnetModel(string tensorFlowModelFilePath)
{
    var pipeline = _mlContext.<preprocess-data>
           .Append(_mlContext.Model.LoadTensorFlowModel(tensorFlowModelFilePath)
                                               .ScoreTensorFlowModel(
                                                      outputColumnNames: new[]{TensorFlowModelSettings.outputTensorName },
                                                      inputColumnNames: new[] { TensorFlowModelSettings.inputTensorName },
                                                      addBatchDimensionInput: false));
 
    ITransformer mlModel = pipeline.Fit(CreateEmptyDataView());
 
    return mlModel;
}
