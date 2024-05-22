/** @type {import('./$types').PageLoad} */
export async function load() {
    const chats=await fetch("http://localhost:5136/api/Chats/all")
    .then(response=>{
        if(!response.ok){
            throw new Error(`Network reponse ${response.text()}`);
        }
        return response.json();
    })
    .catch(error=>{
        console.error('Fetch error:',error);
    });
    /*
    fetch('https://api.example.com/data')
  .then(response => {
    if (!response.ok) {
      throw new Error('Network response was not ok');
    }
    return response.json();
  })
  .then(data => {
    console.log('Data received:', data);
  })
  .catch(error => {
    console.error('There was a problem with the fetch operation:', error);
  });
    */
    return {
        chats
    };
};