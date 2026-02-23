from flask import Flask, request, jsonify
from pypdf import PdfReader
from langchain.text_splitter import CharacterTextSplitter
from langchain.vectorstores import FAISS
from langchain.embeddings import HuggingFaceEmbeddings
from langchain.llms import OpenAI
import os

app = Flask(__name__)

vectorstore = None

@app.route("/upload", methods=["POST"])
def upload_pdf():
    global vectorstore

    file = request.files["file"]
    reader = PdfReader(file)

    text = ""
    for page in reader.pages:
        text += page.extract_text()

    splitter = CharacterTextSplitter(chunk_size=500, chunk_overlap=50)
    chunks = splitter.split_text(text)

    embeddings = HuggingFaceEmbeddings()
    vectorstore = FAISS.from_texts(chunks, embeddings)

    return "PDF processed successfully!"

@app.route("/ask", methods=["POST"])
def ask_question():
    global vectorstore
    question = request.json["question"]

    docs = vectorstore.similarity_search(question, k=3)

    context = "\n".join([doc.page_content for doc in docs])

    llm = OpenAI()
    answer = llm(f"Answer based on context:\n{context}\n\nQuestion: {question}")

    return jsonify({"answer": answer})

if __name__ == "__main__":
    app.run(host="0.0.0.0", port=5000)
