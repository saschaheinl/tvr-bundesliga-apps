"use client";

import { useState } from "react";
import { useAuthState } from "react-firebase-hooks/auth";
import { auth } from "@/firebase"; 
import axios from "axios";
import { outline, Scanner } from "@yudiel/react-qr-scanner";

interface TicketData {
    id: string;
    guestName: string;
    guestEmail?: string | null;
    totalVisits: number;
    remainingVisits: number;
    price: number;
    wasSentByEmail: boolean;
    created: string;
    createdBy: string;
    lastModified: string;
    qrCode: QrCodeDetails;
    scans: Scan[];
}

interface QrCodeDetails {
    locationOfImage: string;
    qrCodeAsBase64: string;
}

interface Scan {
    guests: number;
    username: string;
}

interface GuestData {
    name: string;
    emailAddress?: string;
}

const Scan = () => {
    const [user] = useAuthState(auth); 
    const [ticketData, setTicketData] = useState<TicketData | null>(null);
    const [guestData, setGuestData] = useState<GuestData | null>(null);
    const [error, setError] = useState<string | null>(null);
    const [visitors, setVisitors] = useState<string>('');

    const handleScan = async (ticketId: string | null) => {
        console.log('Handling scan...');
        console.log(ticketId);
        console.log(user);
        if (!user) return;
        if (ticketData) return;

        try {
            const token = await user.getIdToken();
            const response = await axios.get<TicketData>(`${process.env.NEXT_PUBLIC_API_BASE_URL}/tickets/${ticketId}`, {
                headers: { Authorization: `Bearer ${token}` },
            });
            const guestData: GuestData = {
                name: response.data.guestName,
                emailAddress: response.data.guestEmail ?? ''
            }
            setTicketData(response.data);
            setGuestData(guestData);
        } catch (err) {
            setError("Ticket not found or invalid.");
        }
    };
    
    const handleVisitorsSubmit = async () => {
        if (!user) return;

        const remainingVisits = ticketData!.remainingVisits - parseInt(visitors);
        if (remainingVisits < 0) {
            alert(`${Math.abs(remainingVisits)} weiteres Ticket nötig.`);
            return;
        }

        try {
            const token = await user.getIdToken();
            const scan: Scan = {
                guests: parseInt(visitors),
                username: user.email ?? user.displayName ?? ''
            }

            await axios.post(`${process.env.NEXT_PUBLIC_API_BASE_URL}/tickets/${ticketData!.id}/scans`, {
                ...scan,
            }, {
                headers: { Authorization: `Bearer ${token}` },
            });

            setTicketData(null);
        } catch (err) {
            setError("Error updating ticket.");

            return;
        }
    };

    return (
        <div className="flex flex-col justify-center items-center h-screen bg-gray-100 px-4">
            <div className="w-full max-w-md bg-white p-6 rounded-lg shadow-md space-y-4">
                <div className="mb-6">
                    <h2 className="text-xl font-bold mb-2 text-center">Scan Your Ticket</h2>
                    <div className="flex justify-center" style={{ width: "300px", height: "300px" }}>
                        <Scanner
                            components={{ tracker: outline, finder: true }}
                            allowMultiple={true}
                            formats={["qr_code"]}
                            onScan={(result) => handleScan(result[0].rawValue)}
                        />
                    </div>
                </div>
                
                {ticketData && guestData && (
                    <div>
                        <h2 className="text-xl font-bold">Ticket Details</h2>
                        <p>Gast: {guestData.name}</p>
                        <p>Besuche insgesamt: {ticketData.totalVisits}</p>
                        <p><b>Besuche verbleibend: {ticketData.remainingVisits}</b></p>
                        <label className="block mt-4">
                            Number of Visitors:
                            <input
                                type="number"
                                className="w-full p-2 mt-2 border border-gray-300 rounded-lg text-black"
                                min="1"
                                value={visitors} 
                                onChange={(e) => {
                                    const value = e.target.value;
                                    setVisitors(value);}
                                }
                            />
                        </label>
                        <button
                            onClick={handleVisitorsSubmit}
                            className="w-full mt-4 p-2 bg-green-500 text-white rounded-lg hover:bg-green-600 transition"
                        >
                            Submit
                        </button>
                    </div>
                )}
                
                {error && <p className="text-red-500 text-center mt-4">{error}</p>}
            </div>
        </div>
    );
};

export default Scan;
