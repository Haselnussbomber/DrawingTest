import { useState, useEffect } from "react";
import Clock from "./Clock";

export default function App() {
  const [currentTime, setCurrentTime] = useState(new Date().toLocaleTimeString());

  useEffect(() => {
    const intervalId = setInterval(() => {
      setCurrentTime(new Date().toLocaleTimeString());
    }, 100);

    return () => clearInterval(intervalId);
  }, [currentTime]);

  return <div>Hello World! <Clock time={currentTime}/></div>;
}
