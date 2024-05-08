import { v4 as uuidv4 } from "uuid";

const Score = {
  id: uuidv4(),
  audiotrackId: uuidv4(),
  authorId: uuidv4(),
  authorName: "",
  text: ''
};

export default Score;