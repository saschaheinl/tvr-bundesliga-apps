"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import { signInWithEmailAndPassword } from "firebase/auth";
import { auth } from "@/firebase";  // Adjust the path to your Firebase config

const Login = () => {
    const [email, setEmail] = useState<string>("");  // Email state
    const [password, setPassword] = useState<string>("");  // Password state
    const [error, setError] = useState<string | null>(null);  // Error state
    const router = useRouter();  // Next.js navigation router

    const handleLogin = async () => {
        setError(null);
        try {
            await signInWithEmailAndPassword(auth, email, password);  // Firebase login
            router.push("/scan");  // Redirect after successful login
        } catch (err: unknown) {
            if (err instanceof Error){
                setError(err.message);  // Set error message if login fails    
            } else {
                setError('unknown error')
            }
        }
    };

    return (
        <div className="flex flex-col justify-center items-center h-screen bg-blue-500 px-4">
            <form
                className="w-full max-w-md bg-white/80 p-6 rounded-lg shadow-md"
                onSubmit={(e) => {
                    e.preventDefault();
                    handleLogin();
                }}
            >
                <h1 className="text-2xl font-bold mb-4 text-center text-gray-800">Login</h1>
                <input
                    type="email"
                    placeholder="Email"
                    className="w-full p-3 mb-4 border border-gray-300 rounded-lg text-black"  // Input text is black
                    value={email}
                    onChange={(e) => setEmail(e.target.value)}
                />
                <input
                    type="password"
                    placeholder="Password"
                    className="w-full p-3 mb-4 border border-gray-300 rounded-lg text-black"  // Input text is black
                    value={password}
                    onChange={(e) => setPassword(e.target.value)}
                />
                <button
                    type="submit"
                    className="w-full p-3 bg-green-500 text-white rounded-lg hover:bg-green-600 transition"
                >
                    Login
                </button>
                {error && <p className="text-red-500 text-center mt-4">{error}</p>}
            </form>
        </div>
    );
};

export default Login;
