import { useEffect, useState } from "react";

function Hand() {

  const [card, setCard] = useState();

  useEffect(() => {
    (async () => {
      const resource = await fetch("https://localhost:28645/api/Cards?name=JD");
      const data = await resource.text();
      setCard(data);
    })();
    
  }, []);


  return (
    <div>
      <img src={card} alt="Test Card" />
    </div>
  )
}

export default Hand;