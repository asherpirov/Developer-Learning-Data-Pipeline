import pandas as pd
import json
import os

TECH_DOC = "Technical documentation (is generated for/by the tool or system)"
AI_CODEGEN = "AI CodeGen tools or AI-enabled apps"
STACK_OVERFLOW = "Stack Overflow or Stack Exchange"

def main():
    df = load_data("../data/cleaned_data.jsonl")
    df['experienceLevel'] = df['YearsCode'].apply(experience_level)

    df['usesDocumentation'] = df['LearnCode'].apply(uses_documentation)
    df['usesAIForLearning'] = df['LearnCode'].apply(uses_ai_for_learning)
    df['usesStackOverflow'] = df['LearnCode'].apply(uses_stack_overflow)

    print(df.columns)
    print(df[['YearsCode', 'experienceLevel']].head(10))
    print(df['experienceLevel'].value_counts())

    df = df.rename(columns={
        'ResponseId': 'responseId',
        'Age': 'age',
        'YearsCode': 'yearsCode',
        'DevType': 'devType',
        'LearnCodeChoose': 'learnCodeChoose',
        'LearnCode': 'learningMethods',
        'LearnCodeAI': 'learnCodeAI',
        'AILearnHow': 'aiLearningMethods',
        'AISelect': 'aiUsage',
        'AIAcc': 'aiTrust',
        'AISent': 'aiSentiment',
    })
    print(df.columns)
    print(len(df.columns))
    print(df.head())

    save_json(df, "../data/processed_data.jsonl")

def load_data(file_name):
    records = []
    with open(file_name, "r", encoding="utf-8") as f:
        for line in f:
            records.append(json.loads(line))
    df = pd.DataFrame(records)
    return df

def experience_level(years_code):
    if pd.isna(years_code):
        return "Unknown"
    if years_code <= 2:
        return "Beginner"
    if years_code <= 5:
        return "Early Career"
    if years_code <= 10:
        return "Experienced"
    return "Highly Experienced"

def uses_documentation(learn_code_list):
    if type(learn_code_list) is not list:
        return False
    return TECH_DOC in learn_code_list

def uses_ai_for_learning(learn_code_list):
    if type(learn_code_list) is not list:
        return False
    return AI_CODEGEN in learn_code_list

def uses_stack_overflow(learn_code_list):
    if type(learn_code_list) is not list:
        return False
    return STACK_OVERFLOW in learn_code_list

def save_json(df,file_name):
    with open(file_name, "w", encoding="utf-8") as f:
        for _, row in df.iterrows():
            record = row.to_dict()
            for key, value in record.items():
                if not isinstance(value, list) and pd.isna(value):
                    record[key] = None
            f.write(json.dumps(record) + "\n")

if __name__ == "__main__":
    main()