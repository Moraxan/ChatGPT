import React, { useState } from 'react';
import './AppFront.css';

// ... Your existing GPTRequestModel, GPTResponseModel interfaces  ...

function AppFront() {
    const [inputText, setInputText] = useState('');
    const [response, setResponse] = useState(null); // Now hold string

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();

        try {
            const apiResponse = await fetch(`https://localhost:7103/chatgpt/query`, {
                method: 'POST',
                headers: { 'Content-Type': 'application/json' },
                body: JSON.stringify({ Prompt: inputText })
            });

            if (!apiResponse.ok) {
                throw new Error('Error fetching response');
            }

            const data = await apiResponse.json();

            // Defensive extraction 
            const contentToDisplay = data?.Choices?.[0]?.Message?.Content ?? 'An error occurred.';
            setResponse(contentToDisplay);

            // For debugging in the meantime:
            console.log('Full Backend Response:', data);

        } catch (error) {
            console.error('Error:', error);
            setResponse('An error occurred while processing your input.');
        }
    };

    return (
        <div className="app-container">
            <header className="app-header">
                {/* Logo, Navigation...  here later */}
            </header>

            <main className="app-content">
            <h1>Chat with GPT-3</h1>
                <div className="question-box">
                    <form onSubmit={handleSubmit}>
                        <input
                            type="text"
                            value={inputText}
                            onChange={(e) => setInputText(e.target.value)}
                        />
                        <button type="submit">Submit</button>
                    </form>
                </div>
                <h3>Response:</h3>
                <div className="response-box">
                    {response &&
                        <div> {response} </div>  // Render directly  
                    }
                </div>
            </main>

            <footer className="app-footer">
                {/*  Copyright here! */}
            </footer>
        </div>
    );
}

export default AppFront;
