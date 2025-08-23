import { faBan, faXmark } from '@fortawesome/free-solid-svg-icons';
import { FontAwesomeIcon } from '@fortawesome/react-fontawesome';
import { useRef, useState, type SetStateAction } from 'react';

const maxTags = 5;

interface AddTagsToPostProps {
    postTags: string[];
    setPostTags: (value: SetStateAction<string[]>) => void;
    t: (key: string) => string;
}

const AddTagsToPost: React.FC<AddTagsToPostProps> = ({ postTags, setPostTags, t }) => {
    const [showInputTag, setShowInputTag] = useState(false);

    const tagInput = useRef<HTMLInputElement | null>(null);

    const addTag = () => {
        if (tagInput.current?.value.length === 0 || postTags.length >= maxTags) {
            return;
        }

        addTagTemplate();
    }

    const addTagByKey = (code: string) => {
        if (code !== "Enter" || tagInput.current?.value.length === 0 || postTags.length >= maxTags) {
            return;
        }

        addTagTemplate();
    }

    const addTagTemplate = () => {
        if (!tagInput.current) {
            return;
        }

        const currecntTags = Object.assign([], postTags);
        currecntTags.push(tagInput.current.value);

        tagInput.current.value = "";
        setPostTags(currecntTags);
    }

    const removeTag = (index: number) => {
        const currecntTags = Object.assign([], postTags);
        currecntTags.splice(index, 1);

        setPostTags(currecntTags);
    }

    return (
        <div className="tags">
            <div className="tags__add">
                <div className="title" onClick={() => setShowInputTag(!showInputTag)}>{t("Tags")}</div>
                {showInputTag &&
                    <>
                        <input type="text" onBlur={addTag} onKeyDown={(e) => addTagByKey(e.code)} placeholder="Enter tag" ref={tagInput} />
                        <FontAwesomeIcon
                            icon={faBan}
                            onClick={() => setShowInputTag(!showInputTag)}
                        />
                    </>
                }
            </div>
            <ul className="tags__list">
                {postTags.map((tag, index) => (
                    <li key={index}>
                        <div>{tag}</div>
                        <FontAwesomeIcon
                            icon={faXmark}
                            title={t("RemoveTag")}
                            onClick={() => removeTag(index)}
                        />
                    </li>
                ))}
            </ul>
        </div>
    );
}

export default AddTagsToPost;