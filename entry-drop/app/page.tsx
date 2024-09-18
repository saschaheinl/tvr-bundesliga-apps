"use client";

import { useEffect } from "react";
import { useAuthState } from "react-firebase-hooks/auth";
import { useRouter } from "next/navigation";
import { auth } from "@/firebase"; 

const CheckAuth = () => {
    const [user, loading] = useAuthState(auth);  
    const router = useRouter();  

    useEffect(() => {
        if (loading) return;
        
        if (user) {
            router.push("/scan");
        } else {
            router.push("/login");
        }
    }, [user, loading, router]);

    if (loading) {
        return <p>Loading...</p>;
    }

    return null; 
};

export default CheckAuth;
